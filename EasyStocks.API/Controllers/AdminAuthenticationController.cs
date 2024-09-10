namespace EasyStocks.API.Controllers;

//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminAuthenticationController : ControllerBase
{
    private readonly IAdminAuthService _adminAuthService;
    private readonly ILogger<AdminAuthenticationController> _logger;

    public AdminAuthenticationController(IAdminAuthService adminAuthService, ILogger<AdminAuthenticationController> logger)
    {
        _adminAuthService = adminAuthService ?? throw new ArgumentNullException(nameof(adminAuthService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAdminRequest request)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _adminAuthService.RegisterAdminAsync(request);

            if (response.Success)
            {
                _logger.LogInformation("Admin user {Email} created successfully.", response.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", response.Errors));
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while creating admin account with email {Email}.", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the admin account.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAdminRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _adminAuthService.LoginAdminAsync(request);

            if (response.Success)
            {
                _logger.LogInformation("Admin {Email} logged in successfully.", request.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to log in admin: {Errors}", string.Join(", ", response.Errors));
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while logging in admin with email {Email}.", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (string.IsNullOrEmpty(request.Token))
        {
            _logger.LogWarning("Logout request failed. Token is missing.");
            return BadRequest(new { Success = false, Errors = new[] { "Token is required." } });
        }

        var response = await _adminAuthService.LogoutAdminAsync(request.Token);

        if (!response.Success)
        {
            _logger.LogWarning("Logout failed.");
            return BadRequest(response);
        }

        _logger.LogInformation("Logout successful.");
        return Ok(response);
    }
}