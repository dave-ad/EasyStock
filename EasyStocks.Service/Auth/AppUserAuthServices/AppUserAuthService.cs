namespace EasyStocks.Service.UserAuthServices;

public sealed class AppUserAuthService : IAppUserAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly ILogger<AppUserAuthService> _logger;

    public AppUserAuthService(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AppUserAuthService> logger, ITokenService tokenService, ITokenBlacklistService tokenBlacklistService)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService;
        _tokenBlacklistService = tokenBlacklistService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<RegisterResponse>> RegisterAppUserAsync(RegisterUserRequest request)
    {
        var serviceResponse = new ServiceResponse<RegisterResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var user = await CreateUserEntity(request);
                user.UserName = user.Email;

                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("User with email {Email} already exists.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "User already exists";
                    serviceResponse.TechMessage = "A user with the provided email already exists.";
                    return serviceResponse;
                }

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User account {Email} registered successfully.", request.Email);

                    var roleResult = await _userManager.AddToRoleAsync(user, "AppUser");

                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("User {Email} assigned to User role successfully.", request.Email);
                        var token = _tokenService.CreateToken(user);

                        serviceResponse.IsSuccessful = true;
                        serviceResponse.Value = new RegisterResponse
                        {
                            Success = true,
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = token,
                            Errors = null
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Failed to assign user role to {Email}.", request.Email);
                        serviceResponse.IsSuccessful = false;
                        serviceResponse.Error = "Failed to assign user role.";
                        serviceResponse.TechMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "User registration failed.";
                    serviceResponse.TechMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                }

                transaction.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user {Email}.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "An unexpected error occurred.";
                serviceResponse.TechMessage = ex.Message;
            }
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<LoginResponse>> LoginAppUserAsync(LoginUserRequest request)
    {
        var serviceResponse = new ServiceResponse<LoginResponse>();

        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "User not found";
                serviceResponse.TechMessage = "No user found with the provided email.";
                return serviceResponse;
            }

            var isUser = await _userManager.IsInRoleAsync(user, "AppUser");
            if (!isUser)
            {
                _logger.LogWarning("Login failed for email: {Email}. User is not registered.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "User is not registered";
                serviceResponse.TechMessage = "User is not assigned to the 'AppUser' role.";
                return serviceResponse;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (result.Succeeded)
            {
                var token = _tokenService.CreateToken(user);
                _logger.LogInformation("User {Email} logged in successfully.", request.Email);

                serviceResponse.IsSuccessful = true;
                serviceResponse.Value = new LoginResponse
                {
                    Success = true,
                    Email = user.Email,
                    Token = token,
                    Errors = null
                };
            }
            else
            {
                _logger.LogWarning("Failed to log in User {Email}.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "Incorrect password or login failure";
                serviceResponse.TechMessage = "Password sign-in failed.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in user {Email}.", request.Email);
            serviceResponse.IsSuccessful = false;
            serviceResponse.Error = "An unexpected error occurred.";
            serviceResponse.TechMessage = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<LogoutResponse>> LogoutAppUserAsync(LogoutRequest request)
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

            var blacklistingResult = await _tokenBlacklistService.BlacklistTokenAsync(request.Token);
            if (!blacklistingResult)
            {
                _logger.LogError("Failed to blacklist token.");
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "Failed to logout. Try again.";
                serviceResponse.TechMessage = "An error occurred while attempting to blacklist the token.";
                return serviceResponse;
            }

            _logger.LogInformation("User logged out successfully.");

            serviceResponse.IsSuccessful = true;
            serviceResponse.Value = new LogoutResponse
            {
                Success = true,
                Message = "Logout successful."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging out user.");
            serviceResponse.IsSuccessful = false;
            serviceResponse.Error = "An unexpected error occurred.";
            serviceResponse.TechMessage = ex.Message;
        }

        return serviceResponse;
    }


    // Helper Methods
    private async Task<AppUser> CreateUserEntity(RegisterUserRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
        var address = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var nin = NIN.Create(request.NIN);

        var user = AppUser.Create(
            name: fullname,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            gender: request.Gender,
            dateOfBirth: request.DateOfBirth,
            address: address,
            nin: nin
        );

        return user;
    }
}