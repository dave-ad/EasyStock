//namespace EasyStocks.Service.BrokerServices;

//public sealed class BrokerService : IBrokerService
//{
//    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
//    private readonly BrokerValidator _validator;
//    private readonly UserManager<User> _userManager;
//    private readonly ILogger<BrokerService> _logger;

//    public BrokerService(UserManager<User> userManager, IEasyStockAppDbContext easyStockAppDbContext, BrokerValidator validator, ILogger<BrokerService> logger)
//    {
//        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
//        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
//        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    }

//    public async Task<ServiceResponse<BrokerListResponse>> GetAllBrokers()
//    {
//        return await GetBrokersByType(null);
//    }

//    public async Task<ServiceResponse<BrokerResponse>> GetBrokerById(int brokerId)
//    {
//       var resp = new ServiceResponse<BrokerResponse>();

//        try
//        {
//            var broker = await _easyStockAppDbContext.Brokers
//                .Include(b => b.Users)
//                .FirstOrDefaultAsync(b => b.BrokerId == brokerId /*&& !b.Deleted*/); // Exclude deleted brokers

//            if (broker == null)
//            {
//                resp.IsSuccessful = false;
//                resp.Error = "Broker not found.";
//                return resp;
//            }

//            var brokerResponse = new BrokerResponse
//            {
//                BrokerId = broker.BrokerId,
//                BrokerType = broker.BrokerType,
//                StockBrokerLicense = broker.BrokerLicense?.Value,
//                Users = broker.Users.Select(u => new BrokerAdminResponse
//                {
//                    Id = u.Id,
//                    Email = u.Email
//                }).ToList(),
//                CompanyName = broker.CompanyName,
//                CompanyEmail = broker.CompanyEmail?.Value,
//                CompanyMobileNumber = broker.CompanyMobileNumber?.Value,
//                CompanyAddress = new AddressResponse
//                {
//                    StreetNo = broker.CompanyAddress?.StreetNo,
//                    StreetName = broker.CompanyAddress?.StreetName,
//                    City = broker.CompanyAddress?.City,
//                    State = broker.CompanyAddress?.State,
//                    ZipCode = broker.CompanyAddress?.ZipCode
//                },
//                CACRegistrationNumber = broker.CACRegistrationNumber?.Value,
//                DateCertified = broker.DateCertified,
//                BusinessAddress = new AddressResponse
//                {
//                    StreetNo = broker.BusinessAddress?.StreetNo,
//                    StreetName = broker.BusinessAddress?.StreetName,
//                    City = broker.BusinessAddress?.City,
//                    State = broker.BusinessAddress?.State,
//                    ZipCode = broker.BusinessAddress?.ZipCode
//                },
//                ProfessionalQualification = broker.ProfessionalQualification,
//                Status = broker.Status
//            };

//            resp.IsSuccessful = true;
//            resp.Value = brokerResponse;
//        }
//        catch (Exception ex)
//        {
//            resp.IsSuccessful = false;
//            resp.Error = "An error occurred while fetching broker.";
//            resp.TechMessage = ex.Message;
//        }
//        return resp;
//    }

//    public async Task<ServiceResponse<BrokerListResponse>> GetBrokersByType(BrokerRole? brokerType = null)
//    {
//        var resp = new ServiceResponse<BrokerListResponse>();

//        try
//        {
//            IQueryable<Broker> query = _easyStockAppDbContext.Brokers.Include(b => b.Users);

//            if (brokerType.HasValue)
//                query = query.Where(b => b.BrokerType == brokerType.Value);

//            var brokers = await query
//                .ToListAsync();

//            if (brokers == null || !brokers.Any())
//            {
//                resp.IsSuccessful = false;
//                resp.Error = brokerType.HasValue ? $"No {brokerType.ToString()} brokers found." : "No brokers found.";
//                return resp;
//            }

//            var brokerListResponse = new BrokerListResponse
//            {
//                Brokers = brokers.Select(broker => new BrokerResponse
//                {
//                    BrokerId = broker.BrokerId,
//                    BrokerType = broker.BrokerType,
//                    StockBrokerLicense = broker.BrokerLicense?.Value,
//                    CompanyName = broker.CompanyName,
//                    CompanyEmail = broker.CompanyEmail?.Value,
//                    CompanyMobileNumber = broker.CompanyMobileNumber?.Value,
//                    CompanyAddress = new AddressResponse
//                    {
//                        StreetNo = broker.CompanyAddress?.StreetNo,
//                        StreetName = broker.CompanyAddress?.StreetName,
//                        City = broker.CompanyAddress?.City,
//                        State = broker.CompanyAddress?.State,
//                        ZipCode = broker.CompanyAddress?.ZipCode
//                    },
//                    CACRegistrationNumber = broker.CACRegistrationNumber?.Value,
//                    DateCertified = broker.DateCertified,
//                    BusinessAddress = new AddressResponse
//                    {
//                        StreetNo = broker.BusinessAddress?.StreetNo,
//                        StreetName = broker.BusinessAddress?.StreetName,
//                        City = broker.BusinessAddress?.City,
//                        State = broker.BusinessAddress?.State,
//                        ZipCode = broker.BusinessAddress?.ZipCode
//                    },
//                    ProfessionalQualification = broker.ProfessionalQualification,
//                    Status = broker.Status,
//                    Users = broker.Users.Select(user => new BrokerAdminResponse
//                    {
//                        Id = user.Id,
//                        Email = user.Email
//                    }).ToList()
//                }).ToList()
//            };

//            resp.Value = brokerListResponse;
//            resp.IsSuccessful = true;
//        }
//        catch (Exception ex)
//        {
//            resp.IsSuccessful = false;
//            resp.Error = $"An error occurred while fetching {(brokerType.HasValue ? brokerType.ToString() : "all")} brokers.";
//            resp.TechMessage = ex.Message;
//        }

//        return resp;
//    }

//    public async Task<ServiceResponse> ChangeBrokerStatus(int brokerId, AccountStatus newStatus)
//    {
//        var resp = new ServiceResponse();

//        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
//        {
//            try
//            {
//                var broker = await _easyStockAppDbContext.Brokers.FindAsync( brokerId);

//                if (broker == null)
//                {
//                    resp.IsSuccessful = false;
//                    resp.Error = "Broker not found.";
//                    return resp;
//                }

//                broker.UpdateStatus(newStatus);
//                _easyStockAppDbContext.Brokers.Update(broker);
//                await _easyStockAppDbContext.SaveChangesAsync();

//                transaction.Complete();

//                resp.IsSuccessful = true;
//            }
//            catch (Exception ex)
//            {
//                resp.IsSuccessful = false;
//                resp.Error = "An error occurred while updating broker status.";
//                resp.TechMessage = ex.Message;
//            }
//        }

//        return resp;
//    }

//    /// Helper Methods
//    private static ServiceResponse<BrokerIdResponse> CreateDuplicateErrorResponse(ServiceResponse<BrokerIdResponse> resp, string entityType)
//    {
//        resp.Error = $"{entityType} with the provided details already exists.";
//        resp.TechMessage = $"Duplicate Error. A {entityType} with the provided details already exists in the database.";
//        resp.IsSuccessful = false;
//        return resp;
//    }

//    private static ServiceResponse<BrokerIdResponse> CreateDatabaseErrorResponse(ServiceResponse<BrokerIdResponse> resp, Exception ex = null)
//    {
//        resp.Error = "An unexpected error occurred while processing your request.";
//        resp.TechMessage = ex == null ? "Unknown Database Error" : $"Database Error: {ex.GetBaseException().Message}";
//        resp.IsSuccessful = false;
//        return resp;
//    }

//    private static ServiceResponse<BrokerIdResponse> CreateExceptionResponse(ServiceResponse<BrokerIdResponse> resp, Exception ex)
//    {
//        resp.Error = "An unexpected error occurred. Please try again later.";
//        resp.TechMessage = $"Exception: {ex.GetBaseException().Message}";
//        resp.IsSuccessful = false;
//        return resp;
//    }
//}