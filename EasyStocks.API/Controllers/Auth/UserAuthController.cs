namespace EasyStocks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAuthController : ControllerBase
{
    private readonly IAppUserAuthService _userAuthService;
    private readonly ILogger<UserAuthController> _logger;

    public UserAuthController(IAppUserAuthService userAuthService, ILogger<UserAuthController> logger)
    {
        _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _userAuthService.RegisterAppUserAsync(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("User {Email} created successfully.", request.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to create user: {Errors}", string.Join(", ", response.Error));
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while creating user account with email {Email}.", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user account.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _userAuthService.LoginAppUserAsync(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("User {Email} logged in successfully.", request.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to log in user: {Errors}", string.Join(", ", response.Error));
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while logging in user with email {Email}.", request.Email);
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

        var response = await _userAuthService.LogoutAppUserAsync(request);

        if (!response.IsSuccessful)
        {
            _logger.LogWarning("Logout failed.");
            return BadRequest(response);
        }

        _logger.LogInformation("Logout successful.");
        return Ok(response);
    }
}