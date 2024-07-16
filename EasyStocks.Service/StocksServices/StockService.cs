namespace EasyStocks.Service.StocksServices;

public class StockService : IStockService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly StockValidator _validator;
    private readonly ILogger<BrokerService> _logger;
    public StockService(IEasyStockAppDbContext easyStockAppDbContext, StockValidator validator, ILogger<BrokerService> logger)
    {
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<StockIdResponse>> CreateStock(CreateStockRequest request)
    {
        var resp = new ServiceResponse<StockIdResponse>();

        var validationResponse = _validator.ValidateStock(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingStock = await _easyStockAppDbContext.Stocks.AnyAsync(s => s.StockTitle.Trim().ToUpper() == request.StockTitle.ToUpper());

                if (existingStock)
                    return CreateDuplicateErrorResponse(resp, "Stocks");

                var stocks = CreateStockRequestEntity(request);

                var retStocks = _easyStockAppDbContext.Stocks.Add(stocks);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retStocks == null || retStocks.Entity.Id < 1)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new StockIdResponse { Id = retStocks.Entity.Id };
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

    public async Task<ServiceResponse<StockListResponse>> GetAllStocks()
    {
        var resp = new ServiceResponse<StockListResponse>();

        try
        {
            var stocks = await _easyStockAppDbContext.Stocks
                .Select(s => new StockResponse
                {
                    Id = s.Id,
                    StockTitle = s.StockTitle,
                    CompanyName = s.CompanyName,
                    StockType = s.StockType,
                    TotalUnits = s.TotalUnits,
                    PricePerUnit = s.PricePerUnit,
                    OpeningDate = s.OpeningDate,
                    ClosingDate = s.ClosingDate,
                    MinimumPurchase = s.MinimumPurchase,
                    InitialDeposit = s.InitialDeposit,
                    DateListed = s.DateListed,
                    ListedBy = s.ListedBy
                }).ToListAsync();

            resp.Value = new StockListResponse { Stocks = stocks };
            resp.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "An error occurred while retrieving all stocks.");
            //return CreateExceptionResponse(resp, ex);
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while fetching stocks.";
            resp.TechMessage = ex.Message;
        }

        return resp;
    }

    // Helper Methods

    private Stocks CreateStockRequestEntity(CreateStockRequest request)
    {
        var stock = Stocks.Create(
            request.StockTitle,
            request.CompanyName,
            request.StockType,
            request.TotalUnits,
            request.PricePerUnit,
            request.OpeningDate,
            request.ClosingDate,
            request.MinimumPurchase,
            request.DateListed,
            request.ListedBy
            );

        return stock;
    }

    private static ServiceResponse<StockIdResponse> CreateDuplicateErrorResponse(ServiceResponse<StockIdResponse> resp, string entityType)
    {
        resp.Error = $"{entityType} with the provided details already exists.";
        resp.TechMessage = $"Duplicate Error. A {entityType} with the provided details already exists in the database.";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<StockIdResponse> CreateDatabaseErrorResponse(ServiceResponse<StockIdResponse> resp, Exception ex = null)
    {
        resp.Error = "An unexpected error occurred while processing your request.";
        resp.TechMessage = ex == null ? "Unknown Database Error" : $"Database Error: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<StockIdResponse> CreateExceptionResponse(ServiceResponse<StockIdResponse> resp, Exception ex)
    {
        resp.Error = "An unexpected error occurred. Please try again later.";
        resp.TechMessage = $"Exception: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }
}