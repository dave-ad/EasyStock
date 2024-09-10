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

    public async Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request)
    {
        var admin = await CreateAdminEntity(request);
        admin.UserName = admin.Email;

        var existingAdmin = await _userManager.FindByEmailAsync(request.Email);
        if (existingAdmin != null)
        {
            _logger.LogWarning("Admin with email {Email} already exists.", request.Email);
            return new RegisterResponse
            {
                Success = false,
                Errors = new List<string> { "Admin already exists" }
            };
        }

        var result = await _userManager.CreateAsync(admin, request.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("Admin user {Email} created successfully.", request.Email);
            var roleResult = await _userManager.AddToRoleAsync(admin, "Admin");

            if (roleResult.Succeeded)
            {
                _logger.LogInformation("User {Email} assigned to Admin role successfully.", request.Email);
                var token = _tokenService.CreateToken(admin);

                return new RegisterResponse
                {
                    Success = true,
                    UserName = admin.UserName,
                    Email = admin.Email,
                    Token = token,
                    Errors = null
                };
            }
            else
            {
                _logger.LogError("Failed to assign Admin role to user {Email}.", request.Email);
                return new RegisterResponse
                {
                    Success = false,
                    Errors = roleResult.Errors.Select(e => e.Description)
                };
            }
        }
        else
        {
            _logger.LogError("Failed to create admin {Email}.", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return new RegisterResponse
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }

    public async Task<LoginResponse> LoginAdminAsync(LoginAdminRequest request)
    {
        var admin = await _userManager.FindByEmailAsync(request.Email);
        if (admin == null)
        {
            _logger.LogWarning("Admin with email {Email} not found.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Admin not found" }
            };
        }

        var isAdmin = await _userManager.IsInRoleAsync(admin, "Admin");
        if (!isAdmin)
        {
            _logger.LogWarning("Login failed for email: {Email}. User is not a admin.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "User is not an admin" }
            };
        }

        var result = await _signInManager.PasswordSignInAsync(admin.UserName, request.Password, false, false);

        if (result.Succeeded)
        {
            var token = _tokenService.CreateToken(admin); 
            _logger.LogInformation("Admin {Email} logged in successfully.", request.Email);
            return new LoginResponse
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
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Incorrect password or login failure" }
            };
        }
    }

    public async Task<LogoutResponse> LogoutAdminAsync(string token)
    {
        var isTokenBlacklisted = await _tokenBlacklistService.IsTokenBlacklistedAsync(token);
        if (isTokenBlacklisted)
        {
            _logger.LogWarning("Token already blacklisted.");
            return new LogoutResponse
            {
                Success = false,
                Errors = new List<string> { "Token already invalidated"}
            };
        }

        // Invalidate the JWT token by adding it to the blacklist repository
        var blacklistingResult = await _tokenBlacklistService.BlacklistTokenAsync(token);
        if (!blacklistingResult)
        {
            _logger.LogError("Failed to blacklist token.");
            return new LogoutResponse
            {
                Success = false,
                Errors = new List<string> { "Failed to logout. Try again." }
            };
        }

        // Sign out the user (if using cookie-based authentication, you can call _signInManager.SignOutAsync())
        //await _signInManager.SignOutAsync();

        _logger.LogInformation("Admin logged out successfully.");
        return new LogoutResponse
        {
            Success = true,
            Message = "Logout successful."
        };
    }

    // Helper Methods
    private async Task<Admin> CreateAdminEntity(RegisterAdminRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);

        var admin = Admin.Create(
            name: fullname,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            gender: request.Gender
            );

        return admin;
    }
}