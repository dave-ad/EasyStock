namespace EasyStocks.API.Controllers;

//[Authorize(Roles = "Broker")]
[Route("api/[controller]")]
[ApiController]
public class BrokerAuthController : ControllerBase
{
    private readonly IBrokerAuthService _brokerAuthService;
    private readonly ILogger<BrokerAuthController> _logger;

    public BrokerAuthController(IBrokerAuthService brokerAuthService, ILogger<BrokerAuthController> logger)
    {
        _brokerAuthService = brokerAuthService ?? throw new ArgumentNullException(nameof(brokerAuthService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("create")]
    public async Task<IActionResult> RegisterBroker([FromBody] CreateBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for Create Broker request.");
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _brokerAuthService.CreateBrokerAsync(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Broker created successfully.");
                return Ok(new
                {
                    Email = response.Value.Email,
                    Token = response.Value.Token
                });
            }

            _logger.LogWarning("Failed to create broker: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating broker: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginBroker([FromBody] BrokerLoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _brokerAuthService.LoginBrokerAsync(request, BrokerRole.Default);


            if (response.Success)
            {
                _logger.LogInformation("Broker {Email} logged in successfully.", request.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to log in broker: {Errors}", string.Join(", ", response.Errors));
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while logging in broker with email {Email}.", request.Email);
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
            return BadRequest(new 
            { 
                Success = false, 
                Errors = new[] { "Token is required." } 
            });
        }

        var response = await _brokerAuthService.LogoutBrokerAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Logout failed. Errors: {Errors}", string.Join(", ", response.Errors));
            return BadRequest(response);
        }

        _logger.LogInformation("Logout successful.");
        return Ok(response);
    }
}