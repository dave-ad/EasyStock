namespace EasyStocks.API.Controllers;

//[Authorize(Roles = "Broker")]
[Route("api/[controller]")]
[ApiController]
public class BrokerAuthenticationController : ControllerBase
{
    private readonly IBrokerAuthService _brokerAuthService;
    private readonly ILogger<BrokerAuthenticationController> _logger;

    public BrokerAuthenticationController(IBrokerAuthService brokerAuthService, ILogger<BrokerAuthenticationController> logger)
    {
        _brokerAuthService = brokerAuthService ?? throw new ArgumentNullException(nameof(brokerAuthService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("create-corporate-broker")]
    public async Task<IActionResult> RegisterCorporate([FromBody] CreateCorporateBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CreateCorporateBroker request.");
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _brokerAuthService.CreateCorporateBroker(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Corporate broker created successfully.");
                return Ok(new
                {
                    BrokerId = response.Value.BrokerId,
                    Email = response.Value.Email,
                    Token = response.Value.Token
                });
            }

            _logger.LogWarning("Failed to create corporate broker: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating corporate broker: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpPost("create-individual-broker")]
    public async Task<IActionResult> RegisterIndividual([FromBody] CreateIndividualBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CreateCorporateBroker request.");
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _brokerAuthService.CreateIndividualBroker(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Corporate broker created successfully.");
                return Ok(new
                {
                    BrokerId = response.Value.BrokerId,
                    Email = response.Value.Email,
                    Token = response.Value.Token
                });
            }

            _logger.LogWarning("Failed to create corporate broker: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating corporate broker: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }
    
    [HttpPost("create-freelance-broker")]
    public async Task<IActionResult> RegisterFreelance([FromBody] CreateFreelanceBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CreateCorporateBroker request.");
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _brokerAuthService.CreateFreelanceBroker(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Corporate broker created successfully.");
                return Ok(new
                {
                    //BrokerId = response.Value.BrokerId,
                    Email = response.Value.Email,
                    Token = response.Value.Token
                });
            }

            _logger.LogWarning("Failed to create corporate broker: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating corporate broker: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpPost("login-corporate-broker")]
    public async Task<IActionResult> LoginCorporate([FromBody] BrokerLoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _brokerAuthService.LoginCorporateBrokerAsync(request);

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

    [HttpPost("login-individual-broker")]
    public async Task<IActionResult> LoginIndividual([FromBody] BrokerLoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _brokerAuthService.LoginIndividualBrokerAsync(request);

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

    [HttpPost("login-freelance-broker")]
    public async Task<IActionResult> LoginFreelance([FromBody] BrokerLoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var response = await _brokerAuthService.LoginFreelanceBrokerAsync(request);

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
}