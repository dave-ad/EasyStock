using EasyStocks.DTO.Requests;

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

    public async Task<IdentityResult> CreateAdminAsync(RegisterAdminRequest request)
    {
        var admin = Admin.Create(
            FullName.Create(request.FirstName, request.LastName, request.OtherNames),
            request.Email,
            request.PhoneNumber,
            request.Gender
            //request.SuperAdminLevel,
            //request.Permissions
        );
        admin.UserName = admin.Email;

        var existingAdmin = await _userManager.FindByEmailAsync(request.Email);
        if (existingAdmin != null)
        {
            _logger.LogWarning("Admin with email {Email} already exists.", request.Email);
            return IdentityResult.Failed(new IdentityError { Description = "Admin already exists" });
        }

        var result = await _userManager.CreateAsync(admin, request.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("Admin user {Email} created successfully.", request.Email);

            var roleResult = await _userManager.AddToRoleAsync(admin, "Admin");

            if (roleResult.Succeeded)
            {
                _logger.LogInformation("User {Email} assigned to Admin role successfully.", request.Email);
            }
            else
            {
                _logger.LogError("Failed to assign Admin role to user {Email}.", request.Email);
                return roleResult;
            }
        }
        else
        {
            _logger.LogError("Failed to create admin user {Email}.", request.Email);
        }

        return result;
    }

    public async Task<SignInResult> LoginAdminAsync(string email, string password)
    {
        _logger.LogInformation("Attempting to log in admin with email: {Email}", email);

        var admin = await _userManager.FindByEmailAsync(email);
        if (admin == null)
        {
            _logger.LogWarning("Admin with email {Email} not found.", email);
            return SignInResult.Failed;
        }

        //// Check if the user is a broker
        //var isAdmin = await _userManager.IsInRoleAsync(admin, "Broker");
        //if (!isAdmin)
        //{
        //    _logger.LogWarning("Login failed for email: {Email}. User is not a admin.", email);
        //    return SignInResult.Failed;
        //}

        var result = await _signInManager.PasswordSignInAsync(admin.UserName, password, false, false);

        if (result.Succeeded)
        {
            _logger.LogInformation("Admin {Email} logged in successfully.", email);
        }
        else
        {
            _logger.LogWarning("Failed to log in admin {Email}.", email);
        }

        return result;
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

    public async Task<IdentityResult> CreateEasyStockUserAsync(RegisterUserRequest request)
    {
        var user = await RegisterUserEntity(request);
        user.UserName = user.Email;

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", request.Email);
            return IdentityResult.Failed(new IdentityError { Description = "User already exists" });
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Easy Stock User {Email} registered successfully.", request.Email);
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (roleResult.Succeeded)
            {
                _logger.LogInformation("User {Email} assigned to User role successfully.", request.Email);
            }
            else
            {
                _logger.LogWarning("Failed to assign User role to {Email}.", request.Email);
                return roleResult;
            }
        }
        else
        {
            _logger.LogWarning("Failed to register Easy Stock User {Email}. Errors: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result;
    }

    public async Task<SignInResult> LoginEasyStockUserAsync(string email, string password)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", email);

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("User with email {Email} not found.", email);
            return SignInResult.Failed;
        }

        //// Check if the user is an Easy Stock User
        //var isUser = await _userManager.IsInRoleAsync(user, "Broker");
        //if (!isUser)
        //{
        //    _logger.LogWarning("Login failed for email: {Email}. User is not a user.", email);
        //    return SignInResult.Failed;
        //}

        var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

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
        _logger.LogInformation("Logged out successfully.");
    }

    // Helper Methods
    private async Task<EasyStockUser> RegisterUserEntity(RegisterUserRequest request)
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