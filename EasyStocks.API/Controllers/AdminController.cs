using Microsoft.AspNetCore.Authorization;

namespace EasyStocks.API.Controllers;

[Authorize(Roles = "Admin") ]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminAuthService _adminService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminAuthService adminService, ILogger<AdminController> logger)
    {
        _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAdminRequest request)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _adminService.RegisterAdminAsync(request);

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
            _logger.LogError(ex, "An exception occurred while creating admin account for email {Email}.", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the admin account.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAdminRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _adminService.LoginAdminAsync(request);

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
}