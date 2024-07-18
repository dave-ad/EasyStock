namespace EasyStocks.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<BrokerController> _logger;

    public AuthController(IAuthService authService, ILogger<BrokerController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await _authService.LoginUserAsync(model.Email, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                _logger.LogError("Invalid login attempt.");
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
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}