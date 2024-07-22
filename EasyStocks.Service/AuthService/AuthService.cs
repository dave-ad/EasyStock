using Microsoft.AspNetCore.Identity;

namespace EasyStocks.Service.AuthServices;

public sealed class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthService> _logger;

    public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AuthService> logger)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SignInResult> LoginBrokerAdminAsync(string email, string password)
    {
        _logger.LogInformation("Attempting to log in broker with email: {Email}", email);

        // Find user by email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Login failed for email: {Email}. User not found.", email);
            return SignInResult.Failed;
        }

        //// Check if the user is a broker
        //var isBroker = await _userManager.IsInRoleAsync(user, "Broker");
        //if (!isBroker)
        //{
        //    _logger.LogWarning("Login failed for email: {Email}. User is not a broker.", email);
        //    return SignInResult.Failed;
        //}

        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            _logger.LogInformation("Broker {Email} logged in successfully.", email);
        }
        else
        {
            _logger.LogWarning("Failed to log in broker {Email} as admin.", email);
        }

        return result;
    }

    //public async Task<IdentityResult> RegisterEasyStockUserAsync(RegisterEasyStockUserRequest request)
    //{
    //    var user = await RegisterUserEntity(request);

    //    _logger.LogInformation("Attempting to register a new Easy Stock User with email: {Email}", request.Email);

    //    var result = await _easyStockUserManager.CreateAsync(user, request.Password);

    //    if (result.Succeeded)
    //    {
    //        _logger.LogInformation("Easy Stock User {Email} registered successfully.", request.Email);
    //    }
    //    else
    //    {
    //        _logger.LogWarning("Failed to register Easy Stock User {Email}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
    //    }

    //    return result;
    //}

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Logged out successfully.");
    }

    // Helper Methods
    private async Task<EasyStockUser> RegisterUserEntity(RegisterEasyStockUserRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
        var mobileNumber = MobileNo.Create(request.MobileNumber);
        var address = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var nin = NIN.Create(request.NIN);

        var user = EasyStockUser.Create(
            name: fullname,
            email: request.Email,
            mobileNumber: mobileNumber,
            gender: request.Gender,
            dateOfBirth: request.DateOfBirth,
            address: address,
            nin: nin
            );

        return user;
    }
}