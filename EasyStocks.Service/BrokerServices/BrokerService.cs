namespace EasyStocks.Service.BrokerServices;

public sealed class BrokerService : IBrokerService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly BrokerValidator _validator;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BrokerService> _logger;

    public BrokerService(UserManager<User> userManager, IEasyStockAppDbContext easyStockAppDbContext, BrokerValidator validator, ILogger<BrokerService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger;
    }

    public async Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request) 
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateCorporate(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingCorporateBroker = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.CompanyEmail.Value.Trim().ToUpper() == request.CompanyEmail.Trim().ToUpper());

                if (existingCorporateBroker)
                    return CreateDuplicateErrorResponse(resp, "Corporate broker");

                //var existingStaff = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.Email.Value.Trim().ToUpper() == request.Users[0].Email.Trim().ToUpper());

                //if (existingStaff)
                //    return CreateDuplicateErrorResponse(resp, "Staff");

                var duplicateStaff = request.Users.Count > 1 && request.Users[0].Email.Trim().ToUpper() == request.Users[1].Email.Trim().ToUpper();
                if (duplicateStaff)
                    return CreateDuplicateErrorResponse(resp, "Staff");

                //var corporateBroker = await CreateCorporateBrokerEntity(request);
                var corporateBroker =  CreateCorporateBrokerEntity(request);

                var retCorporateBroker = _easyStockAppDbContext.Brokers.Add(corporateBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retCorporateBroker == null || retCorporateBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                // Create users and assign them to the created broker
                await CreateUsersAndAssignToBroker(request.Users, retCorporateBroker.Entity.BrokerId);

                resp.Value = new BrokerIdResponse { BrokerId = retCorporateBroker.Entity.BrokerId };
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
                var existingBroker = await _userManager.FindByEmailAsync(request.Users[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "broker");

                var individualBroker = CreateIndividualBrokerEntity(request);

                var retIndividualBroker = _easyStockAppDbContext.Brokers.Add(individualBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retIndividualBroker == null || retIndividualBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                // Create users and assign them to the created broker
                await CreateUsersAndAssignToBroker(request.Users, retIndividualBroker.Entity.BrokerId);


                resp.Value = new BrokerIdResponse { BrokerId = retIndividualBroker.Entity.BrokerId };
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
                var existingBroker = await _userManager.FindByEmailAsync(request.Users[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "broker");

                //var freelanceBroker = await CreateFreelanceBrokerEntity(request);
                var freelanceBroker = CreateFreelanceBrokerEntity(request);

                var retFreelanceBroker = _easyStockAppDbContext.Brokers.Add(freelanceBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retFreelanceBroker == null || retFreelanceBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                // Create users and assign them to the created broker
                await CreateUsersAndAssignToBroker(request.Users, retFreelanceBroker.Entity.BrokerId);

                resp.Value = new BrokerIdResponse { BrokerId = retFreelanceBroker.Entity.BrokerId };
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

    private async Task CreateUsersAndAssignToBroker(List<UserRequest> userRequests, int brokerId)
    {
        foreach (var userRequest in userRequests)
        {
            var user = new User(
                FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
                userRequest.Email,
                MobileNo.Create(userRequest.MobileNumber),
                userRequest.Gender,
                userRequest.PositionInOrg,
                userRequest.DateOfEmployment)
            {
                BrokerId = brokerId // Assign the created brokerId to the user
            };

            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
                throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
            }

            _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
        }
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
            new List<User>() // Empty list for now
            );

        return broker;
    }

    private Broker CreateIndividualBrokerEntity(CreateIndividualBrokerRequest request)
    {
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicenseNumber);
        var businessAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);

        return Broker.CreateIndividual(new List<User>(), stockBrokerLicense, request.DateCertified, businessAddress, request.ProfessionalQualification);
    }

    private Broker CreateFreelanceBrokerEntity(CreateFreelanceBrokerRequest request)
    {
        return Broker.CreateFreelance(new List<User>(), request.ProfessionalQualification);
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