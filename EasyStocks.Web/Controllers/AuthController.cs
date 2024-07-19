namespace EasyStocks.Web.Controllers;

public class AuthController : Controller
{
    //    private readonly IAuthService _authService;
    //    private readonly ILogger<BrokerController> _logger;

    //    public AuthController(IAuthService authService, ILogger<BrokerController> logger)
    //    {
    //        _authService = authService;
    //        _logger = logger;
    //    }


    //    // Register User
    //    [HttpGet]
    //    public IActionResult Register()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(request);

    //        try
    //        {
    //            var result = await _authService.RegisterUserAsync(request);

    //            if (result.Succeeded)
    //            {
    //                _logger.LogInformation("User registered successfully.");
    //                return RedirectToAction("Login");
    //            }
    //            else
    //            {
    //                _logger.LogWarning("Failed to register user.");
    //                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
    //                return View(request);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "An exception occurred while trying to register.");
    //            ModelState.AddModelError(string.Empty, "An error occurred while trying to register.");
    //            return View(request);
    //        }
    //    }


    //        [HttpGet]
    //    public IActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Login([FromForm] LoginModel model)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(model);

    //        try
    //        {
    //            var result = await _authService.LoginUserAsync(model.Email, model.Password);

    //            if (result.Succeeded)
    //                return RedirectToAction("Index", "Home");
    //            else
    //            {
    //                _logger.LogError("Invalid login attempt.");
    //                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //                return View(model);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "An exception occurred while trying to log in.");
    //            ModelState.AddModelError(string.Empty, "An error occurred while trying to log in.");
    //            return View(model);
    //        }
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Logout()
    //    {
    //        await _authService.LogoutAsync();
    //        return RedirectToAction("Index", "Home");
    //    }
}