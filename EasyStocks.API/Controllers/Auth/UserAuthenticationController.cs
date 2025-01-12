namespace EasyStocks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAuthenticationController : ControllerBase
{
    private readonly IAppUserAuthService _userAuthService;
    private readonly ILogger<UserAuthenticationController> _logger;

    public UserAuthenticationController(IAppUserAuthService userAuthService, ILogger<UserAuthenticationController> logger)
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
            var response = await _userAuthService.RegisterUserAsync(request);

            if (response.Success)
            {
                _logger.LogInformation("User {Email} created successfully.", response.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to create user: {Errors}", string.Join(", ", response.Errors));
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
            var response = await _userAuthService.LoginUserAsync(request);

            if (response.Success)
            {
                _logger.LogInformation("User {Email} logged in successfully.", request.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to log in user: {Errors}", string.Join(", ", response.Errors));
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

        var response = await _userAuthService.LogoutUserAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Logout failed.");
            return BadRequest(response);
        }

        _logger.LogInformation("Logout successful.");
        return Ok(response);
    }
}