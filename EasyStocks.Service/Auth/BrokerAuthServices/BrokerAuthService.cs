namespace EasyStocks.Service.BrokerAuthServices;

public sealed class BrokerAuthService : IBrokerAuthService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly BrokerValidator _validator;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly ILogger<BrokerAuthService> _logger;

    public BrokerAuthService(IEasyStockAppDbContext easyStockAppDbContext, BrokerValidator validator, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<BrokerAuthService> logger, ITokenService tokenService, ITokenBlacklistService tokenBlacklistService)
    {
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tokenBlacklistService = tokenBlacklistService;
    }

    //public async Task<ServiceResponse<RegisterResponse>> CreateBrokerAsync(CreateBrokerRequest request)
    //{
    //    var serviceResponse = new ServiceResponse<RegisterResponse>();

    //    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    //    {
    //        try
    //        {
    //            var broker = await CreateBrokerEntity(request);
    //            broker.UserName = broker.Email;

    //            var existingBroker = await _userManager.FindByEmailAsync(request.Email);
    //            if (existingBroker != null)
    //            {
    //                _logger.LogWarning("Broker with email {Email} already exists.", request.Email);
    //                serviceResponse.IsSuccessful = false;
    //                serviceResponse.Error = "Broker already exists";
    //                serviceResponse.TechMessage = "Broker with the provided email already exists in the system.";
    //                return serviceResponse;
    //            }

    //            var result = await _userManager.CreateAsync(broker, request.Password);
    //            if (!result.Succeeded)
    //            {
    //                _logger.LogWarning("Failed to register broker {Email}. Errors: {Errors}",
    //                    request.Email,
    //                    string.Join(", ", result.Errors.Select(e => e.Description)));
    //                serviceResponse.IsSuccessful = false;
    //                serviceResponse.Error = "Broker registration failed.";
    //                serviceResponse.TechMessage = string.Join(", ", result.Errors.Select(e => e.Description));
    //                return serviceResponse;
    //            }

    //            _logger.LogInformation("Broker account {Email} registered successfully.", request.Email);
    //            var roleResult = await _userManager.AddToRoleAsync(broker, "Broker");
    //            if (!roleResult.Succeeded)
    //            {
    //                _logger.LogWarning("Failed to assign broker role to {Email}.", request.Email);
    //                serviceResponse.IsSuccessful = false;
    //                serviceResponse.Error = "Failed to assign role.";
    //                serviceResponse.TechMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
    //                return serviceResponse;
    //            }

    //            _logger.LogInformation("Broker {Email} assigned to User role successfully.", request.Email);
    //            var token = _tokenService.CreateToken(broker);

    //            serviceResponse.IsSuccessful = true;
    //            serviceResponse.Value = new RegisterResponse
    //            {
    //                Success = true,
    //                UserName = broker.UserName,
    //                Email = broker.Email,
    //                Token = token,
    //                Errors = null
    //            };

    //            transaction.Complete();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "An error occurred while creating broker {Email}.", request.Email);
    //            serviceResponse.IsSuccessful = false;
    //            serviceResponse.Error = "An unexpected error occurred.";
    //            serviceResponse.TechMessage = ex.Message;
    //        }
    //    }

    //    return serviceResponse;
    //}

    public async Task<ServiceResponse<RegisterResponse>> CreateBrokerAsync(CreateBrokerRequest request)
    {
        var serviceResponse = new ServiceResponse<RegisterResponse>();

        // Ensure transaction scope is used correctly
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                // Create broker entity from request
                var broker = await CreateBrokerEntity(request);
                broker.UserName = broker.Email;

                // Check if broker already exists
                var existingBroker = await _userManager.FindByEmailAsync(request.Email);
                if (existingBroker != null)
                {
                    _logger.LogWarning("Broker with email {Email} already exists.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Broker already exists";
                    serviceResponse.TechMessage = "Broker with the provided email already exists in the system.";
                    return serviceResponse;
                }

                // Attempt to create broker
                var result = await _userManager.CreateAsync(broker, request.Password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to register broker {Email}. Errors: {Errors}",
                        request.Email,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Broker registration failed.";
                    serviceResponse.TechMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return serviceResponse;
                }

                _logger.LogInformation("Broker account {Email} registered successfully.", request.Email);

                // Assign Broker role
                var roleResult = await _userManager.AddToRoleAsync(broker, "Broker");
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to assign broker role to {Email}.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Failed to assign role.";
                    serviceResponse.TechMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return serviceResponse;
                }

                _logger.LogInformation("Broker {Email} assigned to Broker role successfully.", request.Email);

                // Generate token
                var token = _tokenService.CreateToken(broker);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Failed to generate token for broker {Email}.", request.Email);
                    serviceResponse.IsSuccessful = false;
                    serviceResponse.Error = "Token generation failed.";
                    return serviceResponse;
                }

                // Set success response
                serviceResponse.IsSuccessful = true;
                serviceResponse.Value = new RegisterResponse
                {
                    Success = true,
                    UserName = broker.UserName,
                    Email = broker.Email,
                    Token = token,
                    Errors = null
                };

                // Commit transaction
                transaction.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating broker {Email}.", request.Email);
                serviceResponse.IsSuccessful = false;
                serviceResponse.Error = "An unexpected error occurred.";
                serviceResponse.TechMessage = ex.Message;
            }
        }

        return serviceResponse;
    }



    public async Task<LoginResponse> LoginBrokerAsync(BrokerLoginRequest request, BrokerRole brokerRole)
    {
        var broker = await _userManager.FindByEmailAsync(request.Email);
        if (broker == null)
        {
            _logger.LogWarning("Broker with email {Email} not found.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Broker not found" }
            };
        }

        var roleName = brokerRole.ToString();
        var isBroker = await _userManager.IsInRoleAsync(broker, roleName);
        if (!isBroker)
        {
            _logger.LogWarning("Login failed for email: {Email}. User is not a {Role}.", request.Email, roleName);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { $"User is not a {roleName}" }
            };
        }

        var result = await _signInManager.PasswordSignInAsync(broker.UserName, request.Password, false, false);

        if (result.Succeeded)
        {
            var token = _tokenService.CreateToken(broker);
            _logger.LogInformation("Broker {Email} logged in successfully as {Role}.", request.Email, roleName);
            return new LoginResponse
            {
                Success = true,
                Email = broker.Email,
                Token = token,
                Errors = null
            };
        }
        else
        {
            _logger.LogWarning("Failed to log in broker {Email}.", request.Email);
            return new LoginResponse
            {
                Success = false,
                Errors = new List<string> { "Incorrect password or login failure" }
            };
        }
    }

    public async Task<LogoutResponse> LogoutBrokerAsync(LogoutRequest request)
    {
        var isTokenBlacklisted = await _tokenBlacklistService.IsTokenBlacklistedAsync(request.Token);
        if (isTokenBlacklisted)
        {
            _logger.LogWarning("Token already blacklisted.");
            return new LogoutResponse
            {
                Success = false,
                Errors = new List<string> { "Token already invalidated" }
            };
        }

        var blacklistingResult = await _tokenBlacklistService.BlacklistTokenAsync(request.Token);
        if (!blacklistingResult)
        {
            _logger.LogError("Failed to blacklist token.");
            return new LogoutResponse
            {
                Success = false,
                Errors = new List<string> { "Failed to logout. Try again." }
            };
        }

        _logger.LogInformation("User logged out successfully.");
        return new LogoutResponse
        {
            Success = true,
            Message = "Logout successful."
        };
    }

    // Helper Methods
    private async Task<Broker> CreateBrokerEntity(CreateBrokerRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);
        var address = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var nin = NIN.Create(request.NIN);
        var brokerLicense = BrokerLicense.Create(request.BrokerLicense);

        var broker = Broker.Create(
            name: fullname,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            gender: request.Gender,
            address: address,
            nin: nin,
            brokerLicense, 
            dateCertified: request.DateCertified,
            professionalQualification: request.ProfessionalQualification, 
            BrokerRole.Default, AccountStatus.Pending
            );

        return broker;
    }

    private static ServiceResponse<BrokerIdResponse> CreateDuplicateErrorResponse(ServiceResponse<BrokerIdResponse> resp, string entityType)
    {
        resp.Error = $"{entityType} with the provided details already exists.";
        resp.TechMessage = $"Duplicate Error. A {entityType} with the provided details already exists in the database.";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<BrokerIdResponse> CreateDatabaseErrorResponse(ServiceResponse<BrokerIdResponse> resp, Exception ex = null)
    {
        resp.Error = "An unexpected error occurred while processing your request.";
        resp.TechMessage = ex == null ? "Unknown Database Error" : $"Database Error: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<BrokerIdResponse> CreateExceptionResponse(ServiceResponse<BrokerIdResponse> resp, Exception ex)
    {
        resp.Error = "An unexpected error occurred. Please try again later.";
        resp.TechMessage = $"Exception: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }
}