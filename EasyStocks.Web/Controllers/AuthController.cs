namespace EasyStocks.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<BrokerController> _logger;
    private readonly ILogger<AuthController> _authlogger;

    public AuthController(IAuthService authService, ILogger<BrokerController> logger, ILogger<AuthController> authlogger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authlogger = authlogger ?? throw new ArgumentNullException(nameof(authlogger));
    }

    [HttpGet]
    public IActionResult LoginBroker()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginBroker([FromForm] LoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authService.LoginBrokerAdminAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Broker {Email} logged in successfully.", model.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError("Invalid login attempt for email {Email}.", model.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to log in.");
            ModelState.AddModelError(string.Empty, "An error occurred while trying to log in.");
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutBrokerAdmin()
    {
        try
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("User logged out successfully.");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to log out.");
            ModelState.AddModelError(string.Empty, "An error occurred while trying to log out.");
            return RedirectToAction("Index", "Home");
        }
    }

    //[HttpGet]
    //public IActionResult RegisterUser()
    //{
    //    return View();
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> RegisterUser([FromForm] RegisterEasyStockUserRequest request)
    //{
    //    if (!ModelState.IsValid)
    //        return View(request);

    //    try
    //    {
    //        var result = await _authService.RegisterEasyStockUserAsync(request);

    //        if (result.Succeeded)
    //        {
    //            _authlogger.LogInformation("User registered successfully.");
    //            return RedirectToAction("LoginBroker");
    //        }
    //        else
    //        {
    //            _authlogger.LogError("User registration failed.");
    //            foreach (var error in result.Errors)
    //            {
    //                ModelState.AddModelError(string.Empty, error.Description);
    //            }
    //            return View(request);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _authlogger.LogError(ex, "An exception occurred while trying to register the user.");
    //        ModelState.AddModelError(string.Empty, "An error occurred while trying to register.");
    //        return View(request);
    //    }
    //}
}