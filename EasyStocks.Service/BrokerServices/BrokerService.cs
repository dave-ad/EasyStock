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

                //var duplicateStaff = request.Users.Count > 1 && request.Users[0].Email.Trim().ToUpper() == request.Users[1].Email.Trim().ToUpper();
                //if (duplicateStaff)
                //    return CreateDuplicateErrorResponse(resp, "Staff");

                var corporateBroker = await CreateCorporateBrokerEntity(request);

                var retCorporateBroker = _easyStockAppDbContext.Brokers.Add(corporateBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retCorporateBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                // Now create users associated with this broker
                var usersCreationResponse = await CreateUsersForBroker(corporateBroker, request.Users);
                if (!usersCreationResponse.IsSuccessful)
                    return usersCreationResponse;

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
                {
                    return CreateDuplicateErrorResponse(resp, "broker");
                }

                var individualBroker = await CreateIndividualBrokerEntity(request);

                var retIndividualBroker = _easyStockAppDbContext.Brokers.Add(individualBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                //if (retIndividualBroker == null || retIndividualBroker.Entity.BrokerId < 1)
                if (retIndividualBroker.Entity.BrokerId < 1)
                {
                    return CreateDatabaseErrorResponse(resp);
                }

                // Now create user associated with this broker
                var userCreationResponse = await CreateUserForBroker(individualBroker, request.Users[0]);
                if (!userCreationResponse.IsSuccessful)
                    return userCreationResponse;

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
                {
                    return CreateDuplicateErrorResponse(resp, "broker");
                }

                var freelanceBroker = await CreateFreelanceBrokerEntity(request);

                var retFreelanceBroker = _easyStockAppDbContext.Brokers.Add(freelanceBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                //if (retFreelanceBroker == null || retFreelanceBroker.Entity.BrokerId < 1)
                if (retFreelanceBroker.Entity.BrokerId < 1)
                {
                    return CreateDatabaseErrorResponse(resp);
                }

                // Now create user associated with this broker
                var userCreationResponse = await CreateUserForBroker(freelanceBroker, request.Users[0]);
                if (!userCreationResponse.IsSuccessful)
                    return userCreationResponse;


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

    private async Task<Broker> CreateCorporateBrokerEntity(CreateCorporateBrokerRequest request)
    {
        //// Validate input data
        //var validationResults = _validator.ValidateCorporate(request);
        //if (!validationResults.IsSuccessful)
        //{
        //    throw new ArgumentException("Invalid request data", nameof(request));
        //}

        //// Create users with ASP.NET Core Identity
        //var users = new List<User>();

        //foreach (var userRequest in request.Users)
        //{
        //    var user = new User(
        //        FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
        //        userRequest.Email,
        //        MobileNo.Create(userRequest.MobileNumber),
        //        userRequest.Gender,
        //        userRequest.PositionInOrg,
        //        userRequest.DateOfEmployment
        //    );

        //    var result = await _userManager.CreateAsync(user, userRequest.Password);
        //    if (result.Succeeded)
        //    {
        //        users.Add(user);
        //        _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
        //    }
        //    else
        //    {
        //        var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
        //        _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
        //        throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
        //    }
        //}


        var companyEmail = Email.Create(request.CompanyEmail);
        var companyMobileNo = MobileNo.Create(request.CompanyMobileNumber);
        var companyAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var cac = CAC.Create(request.CACRegistrationNumber);
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicense);

        var corporateBroker = new Broker(companyEmail, companyMobileNo, companyAddress, cac, stockBrokerLicense);

        return await Task.FromResult(corporateBroker);
        //// Create the broker entity
        //var broker = Broker.CreateCorporate(request.CompanyName, companyEmail, companyMobileNo, companyAddress, cac, stockBrokerLicense, request.DateCertified, users);

        //return broker;
    }

    private async Task<Broker> CreateIndividualBrokerEntity(CreateIndividualBrokerRequest request)
    {
        // Validate input data
        var validationResults = _validator.ValidateIndividual(request);
        if (!validationResults.IsSuccessful) throw new ArgumentException("Invalid request data", nameof(request));

        // Create users with ASP.NET Core Identity
        var users = new List<User>();

        foreach (var userRequest in request.Users)
        {
            var user = new User(
                FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
                userRequest.Email,
                MobileNo.Create(userRequest.MobileNumber),
                userRequest.Gender,
                userRequest.PositionInOrg,
                userRequest.DateOfEmployment
            );

            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (result.Succeeded)
            {
                users.Add(user);
                _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
                throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
            }
        }

        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicenseNumber);
        var businessAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);

        return Broker.CreateIndividual(users, stockBrokerLicense, request.DateCertified, businessAddress, request.ProfessionalQualification);
    }

    private async Task<Broker> CreateFreelanceBrokerEntity(CreateFreelanceBrokerRequest request)
    {
        // Validate input data
        var validationResults = _validator.ValidateFreelance(request);
        if (!validationResults.IsSuccessful) throw new ArgumentException("Invalid request data", nameof(request));

        // Create users with ASP.NET Core Identity
        var users = new List<User>();

        foreach (var userRequest in request.Users)
        {
            var user = new User(
                FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
                userRequest.Email,
                MobileNo.Create(userRequest.MobileNumber),
                userRequest.Gender,
                userRequest.PositionInOrg,
                userRequest.DateOfEmployment
            );

            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (result.Succeeded)
            {
                users.Add(user);
                _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
                throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
            }
        }

        return Broker.CreateFreelance(users, request.ProfessionalQualification);
    }

    private async Task<ServiceResponse> CreateUsersForBroker(Broker broker, List<UserRequest> users)
    {
        var response = new ServiceResponse();

        foreach (var userRequest in users)
        {
            var user = new User(
                FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
                userRequest.Email,
                MobileNo.Create(userRequest.MobileNumber),
                userRequest.Gender,
                userRequest.PositionInOrg,
                userRequest.DateOfEmployment
            );

            // Set UserName to Email
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (result.Succeeded)
            {
                broker.Users.Add(user); // Assuming Broker.Users is a navigation property
                _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
                throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
            }
        }

        await _easyStockAppDbContext.SaveChangesAsync();

        response.IsSuccessful = true;
        return response;
    }

    private async Task<ServiceResponse> CreateUserForBroker(Broker broker, UserRequest userRequest)
    {
        var response = new ServiceResponse();

        var user = new User(
            FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
            userRequest.Email,
            MobileNo.Create(userRequest.MobileNumber),
            userRequest.Gender,
            userRequest.PositionInOrg,
            userRequest.DateOfEmployment
        );

        // Set UserName to Email
        user.UserName = user.Email;

        var result = await _userManager.CreateAsync(user, userRequest.Password);
        if (result.Succeeded)
        {
            broker.Users.Add(user); // Assuming Broker.Users is a navigation property
            _logger.LogInformation("User {Email} registered successfully.", userRequest.Email);
            await _easyStockAppDbContext.SaveChangesAsync();
            response.IsSuccessful = true;
        }
        else
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
            throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
        }

        return response;
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