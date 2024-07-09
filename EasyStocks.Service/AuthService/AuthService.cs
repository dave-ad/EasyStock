namespace EasyStocks.Service.AuthService;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    public async Task<IdentityResult> RegisterUserAsync(UserRequest userRequest)
    {
        var user = new User(
            FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
            userRequest.Email,
            MobileNo.Create(userRequest.MobileNumber),
            userRequest.Gender,
            userRequest.PositionInOrg,
            userRequest.DateOfEmployment
        );

        var result = await _userManager.CreateAsync(user, userRequest.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
        }
        else
        {
            _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result;
    }

    public async Task<SignInResult> LoginUserAsync(string email, string password)
    {
        return await _signInManager.PasswordSignInAsync(email, password, false, false);
    }

    public async Task LogoutAsync()
    {
        // Implement logout logic
    }
}