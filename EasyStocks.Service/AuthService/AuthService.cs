namespace EasyStocks.Service.AuthServices;

public sealed class AuthService : IAuthService
{
    //    private readonly SignInManager<User> _signInManager;
    //    //private readonly SignInManager<BrokerAdmin> signInManager;
    //    private readonly UserManager<User> _userManager;
    //    private readonly ILogger<AuthService> _logger;

    //    public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AuthService> logger)
    //    {
    //        _signInManager = signInManager;
    //        _userManager = userManager;
    //        _logger = logger;
    //    }

    //    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request)
    //    {
    //        var user = await RegisterUserEntity(request);

    //        user.UserName = user.Email;

    //        var result = await _userManager.CreateAsync(user, request.Password);

    //        if (result.Succeeded)
    //            _logger.LogInformation($"User {request.Email} registered successfully.");
    //        else
    //        {
    //            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
    //            _logger.LogWarning($"Failed to register user {request.Email}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //            throw new InvalidOperationException($"User registration failed for {request.Email}. Errors: {errorMessage}");
    //        }

    //        return result;
    //    }

    //    public async Task<SignInResult> LoginUserAsync(string email, string password)
    //    {
    //        _logger.LogInformation("Attempting to log in user with email: {Email}", email);

    //        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

    //        if (result.Succeeded)
    //        {
    //            _logger.LogInformation("User {Email} logged in successfully.", email);
    //        }
    //        else
    //        {
    //            _logger.LogWarning("Failed to log in user {Email}.", email);
    //        }

    //        return result;
    //    }

    //    public async Task LogoutAsync()
    //    {
    //        await _signInManager.SignOutAsync();
    //        _logger.LogInformation("User logged out successfully.");
    //    }

    //    // Helper Methods
    //    private async Task<User> RegisterUserEntity(RegisterRequest request)
    //    {
    //        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
    //        var mobileNumber = MobileNo.Create(request.MobileNumber);

    //        var user = User.Create(
    //            name: fullname,
    //            email: request.Email,
    //            mobileNumber: mobileNumber,
    //            gender: request.Gender
    //            );

    //        return user;
    //    }
}