namespace EasyStocks.Service.AuthServices;

public sealed class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthService> _logger;

    public AuthService(SignInManager<User> signInManager, ILogger<AuthService> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    //public async Task<IdentityResult> RegisterUserAsync(UserRequest userRequest)
    //{
    //    var user = new User(
    //        FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
    //        userRequest.Email,
    //        MobileNo.Create(userRequest.MobileNumber),
    //        userRequest.Gender,
    //        userRequest.PositionInOrg,
    //        userRequest.DateOfEmployment
    //    );

    //    var result = await _userManager.CreateAsync(user, userRequest.Password);

    //    if (result.Succeeded)
    //    {
    //        _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
    //    }
    //    else
    //    {
    //        _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
    //    }

    //    return result;
    //}

    public async Task<SignInResult> LoginUserAsync(string email, string password)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", email);

        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} logged in successfully.", email);
        }
        else
        {
            _logger.LogWarning("Failed to log in user {Email}.", email);
        }

        return result;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out successfully.");
    }
}