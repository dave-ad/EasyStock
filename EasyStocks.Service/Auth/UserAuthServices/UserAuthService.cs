namespace EasyStocks.Service.UserAuthServices;

public sealed class UserAuthService : IUserAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserAuthService> _logger;

    public UserAuthService(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<UserAuthService> logger, ITokenService tokenService)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RegisterResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = await CreateUserEntity(request);
        user.UserName = user.Email;

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", request.Email);
            return new RegisterResponse
            {
                Success = false,
                Errors = new List<string> { "User already exists" }
            };
        }

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User account {Email} registered successfully.", request.Email);
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (roleResult.Succeeded)
            {
                _logger.LogInformation("User {Email} assigned to User role successfully.", request.Email);
                var token = _tokenService.CreateToken(user);

                return new RegisterResponse
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
                _logger.LogWarning("Failed to assign User role to {Email}.", request.Email);
                return new RegisterResponse
                {
                    Success = false,
                    Errors = roleResult.Errors.Select(e => e.Description)
                };
            }
        }
        else
        {
            _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return new RegisterResponse
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }

    public async Task<LoginResponse> LoginUserAsync(LoginUserRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("User with email {Email} not found.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Admin not found" }
            };
        }

        var isUser = await _userManager.IsInRoleAsync(user, "User");
        if (!isUser)
        {
            _logger.LogWarning("Login failed for email: {Email}. User is not a registered.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "User is not registered" }
            };
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

        if (result.Succeeded)
        {
            var token = _tokenService.CreateToken(user);
            _logger.LogInformation("User {Email} logged in successfully.", request.Email);
            return new LoginResponse
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
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Incorrect password or login failure" }
            };
        }
    }

    // Helper Methods
    private async Task<EasyStockUser> CreateUserEntity(RegisterUserRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
        var address = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var nin = NIN.Create(request.NIN);

        var user = EasyStockUser.Create(
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