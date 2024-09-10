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
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<BrokerListResponse>> GetAllBrokers()
    {
        return await GetBrokersByType(null);
    }

    public async Task<ServiceResponse<BrokerResponse>> GetBrokerById(int brokerId)
    {
       var resp = new ServiceResponse<BrokerResponse>();

        try
        {
            var broker = await _easyStockAppDbContext.Brokers
                .Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.BrokerId == brokerId /*&& !b.Deleted*/); // Exclude deleted brokers

            if (broker == null)
            {
                resp.IsSuccessful = false;
                resp.Error = "Broker not found.";
                return resp;
            }

            var brokerResponse = new BrokerResponse
            {
                BrokerId = broker.BrokerId,
                BrokerType = broker.BrokerType,
                StockBrokerLicense = broker.StockBrokerLicense?.Value,
                Users = broker.Users.Select(u => new BrokerAdminResponse
                {
                    Id = u.Id,
                    Email = u.Email
                }).ToList(),
                CompanyName = broker.CompanyName,
                CompanyEmail = broker.CompanyEmail?.Value,
                CompanyMobileNumber = broker.CompanyMobileNumber?.Value,
                CompanyAddress = new AddressResponse
                {
                    StreetNo = broker.CompanyAddress?.StreetNo,
                    StreetName = broker.CompanyAddress?.StreetName,
                    City = broker.CompanyAddress?.City,
                    State = broker.CompanyAddress?.State,
                    ZipCode = broker.CompanyAddress?.ZipCode
                },
                CACRegistrationNumber = broker.CACRegistrationNumber?.Value,
                DateCertified = broker.DateCertified,
                BusinessAddress = new AddressResponse
                {
                    StreetNo = broker.BusinessAddress?.StreetNo,
                    StreetName = broker.BusinessAddress?.StreetName,
                    City = broker.BusinessAddress?.City,
                    State = broker.BusinessAddress?.State,
                    ZipCode = broker.BusinessAddress?.ZipCode
                },
                ProfessionalQualification = broker.ProfessionalQualification,
                Status = broker.Status
            };

            resp.IsSuccessful = true;
            resp.Value = brokerResponse;
        }
        catch (Exception ex)
        {
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while fetching broker.";
            resp.TechMessage = ex.Message;
        }
        return resp;
    }

    public async Task<ServiceResponse<BrokerListResponse>> GetBrokersByType(BrokerRole? brokerType = null)
    {
        var resp = new ServiceResponse<BrokerListResponse>();

        try
        {
            IQueryable<Broker> query = _easyStockAppDbContext.Brokers.Include(b => b.Users);

            if (brokerType.HasValue)
                query = query.Where(b => b.BrokerType == brokerType.Value);

            var brokers = await query
                .ToListAsync();

            if (brokers == null || !brokers.Any())
            {
                resp.IsSuccessful = false;
                resp.Error = brokerType.HasValue ? $"No {brokerType.ToString()} brokers found." : "No brokers found.";
                return resp;
            }

            var brokerListResponse = new BrokerListResponse
            {
                Brokers = brokers.Select(broker => new BrokerResponse
                {
                    BrokerId = broker.BrokerId,
                    BrokerType = broker.BrokerType,
                    StockBrokerLicense = broker.StockBrokerLicense?.Value,
                    CompanyName = broker.CompanyName,
                    CompanyEmail = broker.CompanyEmail?.Value,
                    CompanyMobileNumber = broker.CompanyMobileNumber?.Value,
                    CompanyAddress = new AddressResponse
                    {
                        StreetNo = broker.CompanyAddress?.StreetNo,
                        StreetName = broker.CompanyAddress?.StreetName,
                        City = broker.CompanyAddress?.City,
                        State = broker.CompanyAddress?.State,
                        ZipCode = broker.CompanyAddress?.ZipCode
                    },
                    CACRegistrationNumber = broker.CACRegistrationNumber?.Value,
                    DateCertified = broker.DateCertified,
                    BusinessAddress = new AddressResponse
                    {
                        StreetNo = broker.BusinessAddress?.StreetNo,
                        StreetName = broker.BusinessAddress?.StreetName,
                        City = broker.BusinessAddress?.City,
                        State = broker.BusinessAddress?.State,
                        ZipCode = broker.BusinessAddress?.ZipCode
                    },
                    ProfessionalQualification = broker.ProfessionalQualification,
                    Status = broker.Status,
                    Users = broker.Users.Select(user => new BrokerAdminResponse
                    {
                        Id = user.Id,
                        Email = user.Email
                    }).ToList()
                }).ToList()
            };

            resp.Value = brokerListResponse;
            resp.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            resp.IsSuccessful = false;
            resp.Error = $"An error occurred while fetching {(brokerType.HasValue ? brokerType.ToString() : "all")} brokers.";
            resp.TechMessage = ex.Message;
        }

        return resp;
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

                var corporateBroker =  CreateCorporateBrokerEntity(request);

                var retCorporateBroker = _easyStockAppDbContext.Brokers.Add(corporateBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retCorporateBroker == null || retCorporateBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                await CreateUsersAndAssignToBroker(request.BrokerAdmin, retCorporateBroker.Entity.BrokerId);

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
                var existingBroker = await _userManager.FindByEmailAsync(request.BrokerAdmin[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "broker");

                var individualBroker = CreateIndividualBrokerEntity(request);

                var retIndividualBroker = _easyStockAppDbContext.Brokers.Add(individualBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retIndividualBroker == null || retIndividualBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                await CreateUsersAndAssignToBroker(request.BrokerAdmin, retIndividualBroker.Entity.BrokerId);


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
                var existingBroker = await _userManager.FindByEmailAsync(request.BrokerAdmin[0].Email.Trim());

                if (existingBroker != null)
                    return CreateDuplicateErrorResponse(resp, "broker");

                var freelanceBroker = CreateFreelanceBrokerEntity(request);

                var retFreelanceBroker = _easyStockAppDbContext.Brokers.Add(freelanceBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retFreelanceBroker == null || retFreelanceBroker.Entity.BrokerId < 1)
                    return CreateDatabaseErrorResponse(resp);

                await CreateUsersAndAssignToBroker(request.BrokerAdmin, retFreelanceBroker.Entity.BrokerId);

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

    public async Task<ServiceResponse<BrokerResponse>> UpdateCorporateBroker(UpdateCorporateBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingBrokerResponse = await GetBrokerById(request.BrokerId);

                if (existingBrokerResponse == null || !existingBrokerResponse.IsSuccessful || existingBrokerResponse.Value == null)
                {
                    resp.IsSuccessful = false;
                    resp.Error = "Broker not found.";
                    return resp;
                }

                var existingBroker = await _easyStockAppDbContext.Brokers
                    .Include(b => b.Users)
                    .FirstOrDefaultAsync(b => b.BrokerId == existingBrokerResponse.Value.BrokerId);

                if (existingBroker == null)
                    throw new InvalidOperationException($"Broker with ID {existingBrokerResponse.Value.BrokerId} not found.");

                await UpdateCorporateBrokerEntity(existingBroker, request);

                _easyStockAppDbContext.Brokers.Update(existingBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                var updatedBrokerResponse = new BrokerResponse
                {
                    BrokerId = existingBroker.BrokerId,
                    CompanyName = existingBroker.CompanyName,
                    CompanyEmail = existingBroker.CompanyEmail?.Value,
                    CompanyMobileNumber = existingBroker.CompanyMobileNumber?.Value,
                    CompanyAddress = new AddressResponse
                    {
                        StreetNo = existingBroker.CompanyAddress?.StreetNo,
                        StreetName = existingBroker.CompanyAddress?.StreetName,
                        City = existingBroker.CompanyAddress?.City,
                        State = existingBroker.CompanyAddress?.State,
                        ZipCode = existingBroker.CompanyAddress?.ZipCode
                    },
                    CACRegistrationNumber = existingBroker.CACRegistrationNumber?.Value,
                    StockBrokerLicense = existingBroker.StockBrokerLicense?.Value,
                    DateCertified = existingBroker.DateCertified,
                    //Users = existingBroker.Users.Select(u => new UserResponse
                    //{
                    //    Id = u.Id,
                    //    Email = u.Email
                    //}).ToList(),
                };

                resp.IsSuccessful = true;
                resp.Value = updatedBrokerResponse;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while updating broker.";
                resp.TechMessage = ex.Message;
            }
        }
        
        return resp;
    }

    public async Task<ServiceResponse<BrokerResponse>> UpdateIndividualBroker(UpdateIndividualBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingBrokerResponse = await GetBrokerById(request.BrokerId);

                if (existingBrokerResponse == null || !existingBrokerResponse.IsSuccessful || existingBrokerResponse.Value == null)
                {
                    resp.IsSuccessful = false;
                    resp.Error = "Broker not found.";
                    return resp;
                }

                var existingBroker = await _easyStockAppDbContext.Brokers
                    .Include(b => b.Users)
                    .FirstOrDefaultAsync(b => b.BrokerId == existingBrokerResponse.Value.BrokerId);

                if (existingBroker == null)
                    throw new InvalidOperationException($"Broker with ID {existingBrokerResponse.Value.BrokerId} not found.");

                await UpdateIndividualBrokerEntity(existingBroker, request);

                _easyStockAppDbContext.Brokers.Update(existingBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                var updatedBrokerResponse = new BrokerResponse
                {
                    BrokerId = existingBroker.BrokerId,
                    BusinessAddress = new AddressResponse
                    {
                        StreetNo = existingBroker.BusinessAddress?.StreetNo,
                        StreetName = existingBroker.BusinessAddress?.StreetName,
                        City = existingBroker.BusinessAddress?.City,
                        State = existingBroker.BusinessAddress?.State,
                        ZipCode = existingBroker.BusinessAddress?.ZipCode
                    },
                    StockBrokerLicense = existingBroker.StockBrokerLicense?.Value,
                    DateCertified = existingBroker.DateCertified,
                    ProfessionalQualification = existingBroker.ProfessionalQualification,
                };

                _logger.LogInformation("Updated Broker Response: {@updatedBrokerResponse}", updatedBrokerResponse);

                resp.IsSuccessful = true;
                resp.Value = updatedBrokerResponse;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while updating broker.";
                resp.TechMessage = ex.Message;
            }
        }

        return resp;
    }

    public async Task<ServiceResponse<BrokerResponse>> UpdateFreelanceBroker(UpdateFreelanceBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingBrokerResponse = await GetBrokerById(request.BrokerId);

                if (existingBrokerResponse == null || !existingBrokerResponse.IsSuccessful || existingBrokerResponse.Value == null)
                {
                    resp.IsSuccessful = false;
                    resp.Error = "Broker not found.";
                    return resp;
                }

                var existingBroker = await _easyStockAppDbContext.Brokers
                    .Include(b => b.Users)
                    .FirstOrDefaultAsync(b => b.BrokerId == existingBrokerResponse.Value.BrokerId);

                if (existingBroker == null)
                    throw new InvalidOperationException($"Broker with ID {existingBrokerResponse.Value.BrokerId} not found.");

                await UpdateFreelanceBrokerEntity(existingBroker, request);

                _easyStockAppDbContext.Brokers.Update(existingBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                var updatedBrokerResponse = new BrokerResponse
                {
                    BrokerId = existingBroker.BrokerId,
                    ProfessionalQualification = existingBroker.ProfessionalQualification,
                    //Users = existingBroker.Users.Select(u => new UserResponse
                    //{
                    //    Id = u.Id,
                    //    Email = u.Email
                    //}).ToList(),
                };

                resp.IsSuccessful = true;
                resp.Value = updatedBrokerResponse;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while updating broker.";
                resp.TechMessage = ex.Message;
            }
        }

        return resp;
    }

    public async Task<ServiceResponse> ChangeBrokerStatus(int brokerId, AccountStatus newStatus)
    {
        var resp = new ServiceResponse();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var broker = await _easyStockAppDbContext.Brokers.FindAsync( brokerId);

                if (broker == null)
                {
                    resp.IsSuccessful = false;
                    resp.Error = "Broker not found.";
                    return resp;
                }

                broker.UpdateStatus(newStatus);
                _easyStockAppDbContext.Brokers.Update(broker);
                await _easyStockAppDbContext.SaveChangesAsync();

                transaction.Complete();

                resp.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while updating broker status.";
                resp.TechMessage = ex.Message;
            }
        }

        return resp;
    }

    /// Helper Methods

    private async Task<List<BrokerAdminResponse>> GetUsersForBroker(List<BrokerAdmin> users)
    {
        var userResponses = new List<BrokerAdminResponse>();

        foreach (var user in users)
        {
            var userResponse = new BrokerAdminResponse
            {
                Id = user.Id,
                Email = user.Email
            };

            userResponses.Add(userResponse);
        }

        return userResponses;
    }

    private async Task CreateUsersAndAssignToBroker(List<BrokerAdminRequest> userRequests, int brokerId)
    {
        foreach (var userRequest in userRequests)
        {
            //var user = new BrokerAdmin(
            //    FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
            //    userRequest.Email,
            //    MobileNo.Create(userRequest.MobileNumber),
            //    userRequest.Gender,
            //    userRequest.PositionInOrg,
            //    userRequest.DateOfEmployment,
            //    AccountStatus.Pending)
            //{
            //    BrokerId = brokerId,
            //};
            //user.UserName = user.Email;

            var brokerAdmin = BrokerAdmin.Create(
                FullName.Create(userRequest.FirstName, userRequest.LastName, userRequest.OtherNames),
                userRequest.Email,
                userRequest.PhoneNumber,
                userRequest.Gender,
                userRequest.PositionInOrg,
                userRequest.DateOfEmployment
            );
            brokerAdmin.BrokerId = brokerId;
            brokerAdmin.UserName = brokerAdmin.Email;

            var existingBrokerAdmin = await _userManager.FindByEmailAsync(userRequest.Email);
            if (existingBrokerAdmin != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", userRequest.Email);
                continue; // Skip creating user if it already exists
            }

            var result = await _userManager.CreateAsync(brokerAdmin, userRequest.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to register user {Email}. Errors: {Errors}", userRequest.Email, errorMessage);
                throw new InvalidOperationException($"User registration failed for {userRequest.Email}. Errors: {errorMessage}");
            }

            // Assign the BrokerAdmin role to the user
            var roleResult = await _userManager.AddToRoleAsync(brokerAdmin, "BrokerAdmin");
            if (!roleResult.Succeeded)
            {
                var roleErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to assign BrokerAdmin role to user {Email}. Errors: {Errors}", userRequest.Email, roleErrorMessage);
                throw new InvalidOperationException($"Role assignment failed for {userRequest.Email}. Errors: {roleErrorMessage}");
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

    private async Task UpdateCorporateBrokerEntity(Broker existingBroker, UpdateCorporateBrokerRequest request)
    {
        var companyEmail = Email.Update(request.CompanyEmail);
        var companyMobileNo = MobileNo.Update(request.CompanyMobileNumber);
        var companyAddress = Address.Update(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var cac = CAC.Update(request.CACRegistrationNumber);
        var stockBrokerLicense = StockBrokerLicense.Update(request.StockBrokerLicense);

        existingBroker.UpdateCorporate(
            request.CompanyName,
            companyEmail,
            companyMobileNo,
            companyAddress,
            cac,
            stockBrokerLicense,
            request.DateCertified
        );
    }

    private async Task UpdateIndividualBrokerEntity(Broker existingBroker, UpdateIndividualBrokerRequest request)
    {
        var businessAddress = Address.Update(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var stockBrokerLicense = StockBrokerLicense.Update(request.StockBrokerLicense);

        existingBroker.UpdateIndividual(
            businessAddress,
            stockBrokerLicense,
            request.DateCertified,
            request.ProfessionalQualification
        );
    }
    
    private async Task UpdateFreelanceBrokerEntity(Broker existingBroker, UpdateFreelanceBrokerRequest request)
    {
        existingBroker.UpdateFreelance(
            request.ProfessionalQualification
        );
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