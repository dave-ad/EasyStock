using EasyStocks.Domain.Entities;

namespace EasyStocks.Service.AdminAuthServices;

public sealed class AdminAuthService : IAdminAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly ILogger<AdminAuthService> _logger;

    public AdminAuthService(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AdminAuthService> logger, ITokenService tokenService, ITokenBlacklistService tokenBlacklistService)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService;
        _tokenBlacklistService = tokenBlacklistService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<RegisterResponse>> CreateAdminAsync(CreateAdminRequest request)
    {
        var serviceResponse = new ServiceResponse<RegisterResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var admin = await CreateAdminEntity(request);
                admin.UserName = admin.Email;


                var existingAdmin = await _userManager.FindByEmailAsync(request.Email);
                if (existingAdmin != null)
                {
                    _logger.LogWarning("Admin with email {Email} already exists.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Admin already exists";
                    serviceResponse.TechMessage = "Admin with the provided email already exists in the system.";
                    return serviceResponse;
                }

                var result = await _userManager.CreateAsync(admin, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogWarning("Failed to assign Admin role to user {Email}}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "admin registration failed.";
                    serviceResponse.TechMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return serviceResponse;
                }

                _logger.LogInformation("Admin user {Email} created successfully.", request.Email);
                var roleResult = await _userManager.AddToRoleAsync(admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to assign Admin role to user {Email}.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Failed to assign role.";
                    serviceResponse.TechMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return serviceResponse;
                }

                _logger.LogInformation("User {Email} assigned to Admin role successfully.", request.Email);

                var token = _tokenService.CreateToken(admin);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Failed to generate token for admin {Email}.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Token generation failed.";
                    return serviceResponse;
                }

                serviceResponse.IsSuccessful = true;
                serviceResponse.Value = new RegisterResponse
                {
                    Success = true,
                    UserName = admin.UserName,
                    Email = admin.Email,
                    Token = token,
                    Errors = null
                };

                transaction.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating admin {Email}.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "An unexpected error occurred.";
                serviceResponse.TechMessage = ex.Message;
            }
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<LoginResponse>> LoginAdminAsync(LoginAdminRequest request)
    {
        var serviceResponse = new ServiceResponse<LoginResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var admin = await _userManager.FindByEmailAsync(request.Email);
                if (admin == null)
                {
                    _logger.LogWarning("Admin with email {Email} not found.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Admin not found";
                    serviceResponse.TechMessage = "No admin found with the provided email.";
                    return serviceResponse;
                }

                var isAdmin = await _userManager.IsInRoleAsync(admin, "Admin");
                if (!isAdmin)
                {
                    _logger.LogWarning("Login failed for email: {Email}. User is not an admin.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "User is not an admin";
                    serviceResponse.TechMessage = $"Admin with email {request.Email} is not assigned to the Admin role.";
                    return serviceResponse;
                }

                var result = await _signInManager.PasswordSignInAsync(admin.UserName, request.Password, false, false);

                if (result.Succeeded)
                {
                    var token = _tokenService.CreateToken(admin);
                    _logger.LogInformation("Admin {Email} logged in successfully.", request.Email);

                    serviceResponse.IsSuccessful = true;
                    serviceResponse.Value = new LoginResponse
                    {
                        Success = true,
                        Email = admin.Email,
                        Token = token,
                        Errors = null
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to log in admin {Email}.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Incorrect password or login failure";
                    serviceResponse.TechMessage = "Password sign-in failed.";
                }

                transaction.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in admin {Email}.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "An unexpected error occurred.";
                serviceResponse.TechMessage = ex.Message;
            }
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<LogoutResponse>> LogoutAdminAsync(LogoutRequest request)
    {
        var serviceResponse = new ServiceResponse<LogoutResponse>();

        try
        {
            var isTokenBlacklisted = await _tokenBlacklistService.IsTokenBlacklistedAsync(request.Token);
            if (isTokenBlacklisted)
            {
                _logger.LogWarning("Token already blacklisted.");
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "Token already invalidated";
                serviceResponse.TechMessage = "The provided token has already been blacklisted.";
                return serviceResponse;
            }

            // Invalidate the JWT token by adding it to the blacklist repository
            var blacklistingResult = await _tokenBlacklistService.BlacklistTokenAsync(request.Token);
            if (!blacklistingResult)
            {
                _logger.LogError("Failed to blacklist token.");
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "Failed to logout. Try again.";
                serviceResponse.TechMessage = "An error occurred while attempting to blacklist the token.";
                return serviceResponse;
            }

            // Optionally, sign out the user if using cookie-based authentication
            // await _signInManager.SignOutAsync();

            _logger.LogInformation("Admin logged out successfully.");

            serviceResponse.IsSuccessful = true;
            serviceResponse.Value = new LogoutResponse
            {
                Success = true,
                Message = "Logout successful."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging out admin.");
            serviceResponse.IsSuccessful = false;
            serviceResponse.Error = "An unexpected error occurred.";
            serviceResponse.TechMessage = ex.Message;
        }

        return serviceResponse;
    }

    // Helper Methods
    private async Task<Admin> CreateAdminEntity(CreateAdminRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
        var address = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var nin = NIN.Create(request.NIN);

        var admin = Admin.Create(
            name: fullname,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            gender: request.Gender,
            address,
            nin
            );

        return admin;
    }
}