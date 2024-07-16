namespace EasyStocks.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return View(model);
            }

            var result = await _authService.LoginUserAsync(model.Email, model.Password);
            if (result.Succeeded)
            {
                //return Ok("Login successful");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //return Unauthorized("Invalid login attempt");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            //return Ok(new { message = "Logout successful" });
            return RedirectToAction("Index", "Home");
        }
    }
}
