//using EasyStocks.Service.AuthService;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;

//namespace EasyStocks.Web.Controllers
//{
//    public class AuthController : Controller
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }

//        // GET: AuthController
//        public ActionResult Index()
//        {
//            return View();
//        }

//        // GET: AuthController/Details/5
//        public ActionResult Details(int id)
//        {
//            return View();
//        }

//        // GET: AuthController/Create
//        public ActionResult Register()
//        {
//            return View();
//        }

//        // POST: AuthController/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("register")]
//        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _authService.RegisterUserAsync(userRequest);
//            if (result.Succeeded)
//            {
//                return Ok("Registration successful");
//            }
//            else
//            {
//                return BadRequest(result.Errors);
//            }
//        }

//        // GET: AuthController/Edit/5
//        public ActionResult Edit(int id)
//        {
//            return View();
//        }

//        // POST: AuthController/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _authService.LoginUserAsync(loginRequest.Email, loginRequest.Password);
//            if (result.Succeeded)
//            {
//                return Ok("Login successful");
//            }
//            else
//            {
//                return Unauthorized("Invalid login attempt");
//            }
//        }

//        // GET: AuthController/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }

//        // POST: AuthController/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(int id, IFormCollection collection)
//        {
//            try
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }
//    } 
//}
