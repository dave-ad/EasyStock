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

    public async Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        // Check if maximum limit of 20 brokers is reached
        var currentBrokerCount = await _easyStockAppDbContext.Brokers.CountAsync();
        if (currentBrokerCount >= 20)
        {
            resp.Error = "Cannot create account. Please contact admin for help.";
            resp.IsSuccessful = false;
            _logger.LogError("Maximum limit of broker accounts reached. Cannot create more.");
            return resp;
        }

        var validationResponse = _validator.ValidateCorporate(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingCorporateBroker = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.CompanyEmail.Value.Trim().ToUpper() == request.CompanyEmail.Trim().ToUpper());
                if (existingCorporateBroker)
                    return CreateDuplicateErrorResponse(resp, "Corporate broker");

                var duplicateStaff = request.BrokerAdmin.Count > 1 && request.BrokerAdmin[0].Email.Trim().ToUpper() == request.BrokerAdmin[1].Email.Trim().ToUpper();
                if (duplicateStaff)
                    return CreateDuplicateErrorResponse(resp, "Staff");

                var corporateBroker = CreateCorporateBrokerEntity(request);

                var retCorporateBroker = _easyStockAppDbContext.Brokers.Add(corporateBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retCorporateBroker == null || retCorporateBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                var registerResponse = await CreateUsersAndAssignToCorporateBroker(request.BrokerAdmin, retCorporateBroker.Entity.BrokerId);

                if (!registerResponse.Success)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new BrokerIdResponse
                {
                    BrokerId = retCorporateBroker.Entity.BrokerId,
                    Email = registerResponse.Email,
                    Token = registerResponse.Token
                };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (ArgumentException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (InvalidOperationException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (Exception ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
        }
        return resp;
    }

    public async Task<ServiceResponse<BrokerIdResponse>> CreateIndividualBroker(CreateIndividualBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateIndividual(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingBroker = await _userManager.FindByEmailAsync(request.BrokerAdmin[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "Individual broker");

                var individualBroker = CreateIndividualBrokerEntity(request);

                var retIndividualBroker = _easyStockAppDbContext.Brokers.Add(individualBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retIndividualBroker == null || retIndividualBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                var registerResponse = await CreateUsersAndAssignToIndividualBroker(request.BrokerAdmin, retIndividualBroker.Entity.BrokerId);

                if (!registerResponse.Success)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new BrokerIdResponse 
                { 
                    BrokerId = retIndividualBroker.Entity.BrokerId,
                    Email = registerResponse.Email,
                    Token = registerResponse.Token
                };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (ArgumentException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (InvalidOperationException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (Exception ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
        }
        return resp;
    }

    public async Task<ServiceResponse<BrokerIdResponse>> CreateFreelanceBroker(CreateFreelanceBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateFreelance(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            try
            {
                var existingBroker = await _userManager.FindByEmailAsync(request.BrokerAdmin[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "broker");

                var freelanceBroker = CreateFreelanceBrokerEntity(request);

                var retFreelanceBroker = _easyStockAppDbContext.Brokers.Add(freelanceBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retFreelanceBroker == null || retFreelanceBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                var registerResponse = await CreateUsersAndAssignToFreelanceBroker(request.BrokerAdmin, retFreelanceBroker.Entity.BrokerId);

                if (!registerResponse.Success)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new BrokerIdResponse
                {
                    BrokerId = retFreelanceBroker.Entity.BrokerId,
                    Email = registerResponse.Email,
                    Token = registerResponse.Token
                };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (ArgumentException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (InvalidOperationException ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
            catch (Exception ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
        }
        return resp;
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
    private async Task<RegisterResponse> CreateUsersAndAssignToCorporateBroker(List<BrokerAdminRequest> brokerRequests, int brokerId)
    {
        foreach (var brokerRequest in brokerRequests)
        {
            var broker = await CreateBrokerEntity(brokerRequest);

            broker.BrokerId = brokerId;
            broker.UserName = broker.Email;

            var existingBrokerAdmin = await _userManager.FindByEmailAsync(brokerRequest.Email);
            if (existingBrokerAdmin != null)
            {
                _logger.LogWarning("Broker with email {Email} already exists.", brokerRequest.Email);
                return new RegisterResponse
                {
                    Success = false,
                    Errors = new List<string> { "Broker already exists" }
                };
            }

            var result = await _userManager.CreateAsync(broker, brokerRequest.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Broker account {Email} registered successfully.", brokerRequest.Email);
                var roleResult = await _userManager.AddToRoleAsync(broker, "CorporateBroker");


                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("Broker {Email} assigned to User role successfully.", brokerRequest.Email);
                    var token = _tokenService.CreateToken(broker);

                    return new RegisterResponse
                    {
                        Success = true,
                        UserName = broker.UserName,
                        Email = broker.Email,
                        Token = token,
                        Errors = null
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to assign broker role to {Email}.", brokerRequest.Email);
                    return new RegisterResponse
                    {
                        Success = false,
                        Errors = roleResult.Errors.Select(e => e.Description)
                    };
                }
            }
            else
            {
                _logger.LogWarning("Failed to register broker {Email}. Errors: {Errors}", brokerRequest.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return new RegisterResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
        }
        return new RegisterResponse
        {
            Success = false,
            Errors = new List<string> { "No brokers were processed." }
        };
    }

    private async Task<RegisterResponse> CreateUsersAndAssignToIndividualBroker(List<BrokerAdminRequest> brokerRequests, int brokerId)
    {
        foreach (var brokerRequest in brokerRequests)
        {
            var broker = await CreateBrokerEntity(brokerRequest);

            broker.BrokerId = brokerId;
            broker.UserName = broker.Email;

            var existingBrokerAdmin = await _userManager.FindByEmailAsync(brokerRequest.Email);
            if (existingBrokerAdmin != null)
            {
                _logger.LogWarning("Broker with email {Email} already exists.", brokerRequest.Email);
                return new RegisterResponse
                {
                    Success = false,
                    Errors = new List<string> { "Broker already exists" }
                };
            }

            var result = await _userManager.CreateAsync(broker, brokerRequest.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Broker account {Email} registered successfully.", brokerRequest.Email);
                var roleResult = await _userManager.AddToRoleAsync(broker, "IndividualBroker");


                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("Broker {Email} assigned to User role successfully.", brokerRequest.Email);
                    var token = _tokenService.CreateToken(broker);

                    return new RegisterResponse
                    {
                        Success = true,
                        UserName = broker.UserName,
                        Email = broker.Email,
                        Token = token,
                        Errors = null
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to assign broker role to {Email}.", brokerRequest.Email);
                    return new RegisterResponse
                    {
                        Success = false,
                        Errors = roleResult.Errors.Select(e => e.Description)
                    };
                }
            }
            else
            {
                _logger.LogWarning("Failed to register broker {Email}. Errors: {Errors}", brokerRequest.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return new RegisterResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
        }
        return new RegisterResponse
        {
            Success = false,
            Errors = new List<string> { "No brokers were processed." }
        };
    }

    private async Task<RegisterResponse> CreateUsersAndAssignToFreelanceBroker(List<BrokerAdminRequest> brokerRequests, int brokerId)
    {
        foreach (var brokerRequest in brokerRequests)
        {
            var broker = await CreateBrokerEntity(brokerRequest);

            broker.BrokerId = brokerId;
            broker.UserName = broker.Email;

            var existingBrokerAdmin = await _userManager.FindByEmailAsync(brokerRequest.Email);
            if (existingBrokerAdmin != null)
            {
                _logger.LogWarning("Broker with email {Email} already exists.", brokerRequest.Email);
                return new RegisterResponse
                {
                    Success = false,
                    Errors = new List<string> { "Broker already exists" }
                };
            }

            var result = await _userManager.CreateAsync(broker, brokerRequest.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Broker account {Email} registered successfully.", brokerRequest.Email);
                var roleResult = await _userManager.AddToRoleAsync(broker, "FreelanceBroker");


                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("Broker {Email} assigned to User role successfully.", brokerRequest.Email);
                    var token = _tokenService.CreateToken(broker);

                    return new RegisterResponse
                    {
                        Success = true,
                        UserName = broker.UserName,
                        Email = broker.Email,
                        Token = token,
                        Errors = null
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to assign broker role to {Email}.", brokerRequest.Email);
                    return new RegisterResponse
                    {
                        Success = false,
                        Errors = roleResult.Errors.Select(e => e.Description)
                    };
                }
            }
            else
            {
                _logger.LogWarning("Failed to register broker {Email}. Errors: {Errors}", brokerRequest.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return new RegisterResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
        }
        return new RegisterResponse
        {
            Success = false,
            Errors = new List<string> { "No brokers were processed." }
        };
    }

    private async Task<BrokerAdmin> CreateBrokerEntity(BrokerAdminRequest request)
    {
        var fullname = FullName.Create(request.FirstName, request.LastName, request.OtherNames);

        var brokerAdmin = BrokerAdmin.Create(
            name: fullname,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            gender: request.Gender,
            positionInOrg: request.PositionInOrg,
            dateOfEmployment: request.DateOfEmployment
        );

        return brokerAdmin;
    }

    private Broker CreateCorporateBrokerEntity(CreateCorporateBrokerRequest request)
    {
        var companyEmail = Email.Create(request.CompanyEmail);
        var companyMobileNo = MobileNo.Create(request.CompanyMobileNumber);
        var companyAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var cac = CAC.Create(request.CACRegistrationNumber);
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicense);
        var broker = Broker.CreateCorporate(
            request.CompanyName,
            companyEmail,
            companyMobileNo,
            companyAddress,
            cac,
            stockBrokerLicense,
            request.DateCertified,
            new List<BrokerAdmin>() // Empty list for now
            );

        return broker;
    }

    private Broker CreateIndividualBrokerEntity(CreateIndividualBrokerRequest request)
    {
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicenseNumber);
        var businessAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);

        return Broker.CreateIndividual(new List<BrokerAdmin>(), stockBrokerLicense, request.DateCertified, businessAddress, request.ProfessionalQualification);
    }

    private Broker CreateFreelanceBrokerEntity(CreateFreelanceBrokerRequest request)
    {
        return Broker.CreateFreelance(new List<BrokerAdmin>(), request.ProfessionalQualification);
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