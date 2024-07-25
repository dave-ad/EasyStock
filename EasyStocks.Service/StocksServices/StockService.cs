namespace EasyStocks.Service.StocksServices;

public class StockService : IStockService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly StockValidator _validator;
    private readonly ILogger<StockService> _logger;
    public StockService(IEasyStockAppDbContext easyStockAppDbContext, StockValidator validator, ILogger<StockService> logger)
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

                var stocks = CreateStockEntity(request);

                var retStocks = _easyStockAppDbContext.Stocks.Add(stocks);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retStocks == null || retStocks.Entity.StockId < 1)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new StockIdResponse { Id = retStocks.Entity.StockId };
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
                    StockId = s.StockId,
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
            _logger.LogError(ex, "An error occurred while retrieving all stocks.");
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while fetching stocks.";
            resp.TechMessage = ex.Message;
        }

        return resp;
    }

    public async Task<ServiceResponse<StockResponse>> GetStockById(int stockId)
    {
        var resp = new ServiceResponse<StockResponse>();
        try
        {
            var stock = await _easyStockAppDbContext.Stocks
                .FirstOrDefaultAsync(s => s.StockId == stockId);

            if (stock == null)
            {
                resp.IsSuccessful = false;
                resp.Error = "Stock not found.";
                return resp;
            }

            var stockResponse = new StockResponse
            {
                StockId = stockId,
                StockTitle = stock.StockTitle,
                CompanyName = stock.CompanyName,
                StockType = stock.StockType,
                TotalUnits = stock.TotalUnits,
                PricePerUnit = stock.PricePerUnit,
                OpeningDate = stock.OpeningDate,
                ClosingDate = stock.ClosingDate,
                MinimumPurchase = stock.MinimumPurchase,
                InitialDeposit = stock.InitialDeposit,
                DateListed = stock.DateListed,
                ListedBy = stock.ListedBy
            };

            resp.IsSuccessful = true;
            resp.Value = stockResponse;
        }
        catch (Exception ex)
        {
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while fetching stock.";
            resp.TechMessage = ex.Message;
        }
        return resp;
    }

    public async Task<ServiceResponse<StockResponse>> UpdateStock(UpdateStockRequest request)
    {
        var resp = new ServiceResponse<StockResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingStockResponse = await GetStockById(request.Id);

                if (existingStockResponse == null || !existingStockResponse.IsSuccessful || existingStockResponse.Value == null)
                {
                    resp.IsSuccessful = false;
                    resp.Error = "Stock not found.";
                    return resp;
                }

                var existingStock = await _easyStockAppDbContext.Stocks
                    .FirstOrDefaultAsync(b => b.StockId == existingStockResponse.Value.StockId);

                if (existingStock == null)
                    throw new InvalidOperationException($"Stock with ID {existingStockResponse.Value.StockId} not found.");

                await UpdateStockEntity(existingStock, request);

                _easyStockAppDbContext.Stocks.Update(existingStock);
                await _easyStockAppDbContext.SaveChangesAsync();

                var updatedStockResponse = new StockResponse
                {
                    StockId = existingStock.StockId,
                    StockTitle = existingStock.StockTitle,
                    CompanyName = existingStock.CompanyName,
                    StockType = existingStock.StockType,
                    TotalUnits = existingStock.TotalUnits,
                    PricePerUnit = existingStock.PricePerUnit,
                    OpeningDate = existingStock.OpeningDate,
                    ClosingDate = existingStock.ClosingDate,
                    MinimumPurchase = existingStock.MinimumPurchase,
                    InitialDeposit = existingStock.InitialDeposit,
                    DateListed = existingStock.DateListed,
                    ListedBy = existingStock.ListedBy
                };

                resp.IsSuccessful = true;
                resp.Value = updatedStockResponse;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while updating stock.";
                resp.TechMessage = ex.Message;
            }
        }

        return resp;
    }

    public async Task<ServiceResponse<DeleteStockResponse>> DeleteStock(int stockId)
    {
        var resp = new ServiceResponse<DeleteStockResponse>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var stock = await _easyStockAppDbContext.Stocks.FindAsync(stockId);

                if (stock == null)
                {
                    resp.Error = "Stock not found.";
                    resp.IsSuccessful = false;
                    return resp;
                }

                _easyStockAppDbContext.Stocks.Remove(stock);
                await _easyStockAppDbContext.SaveChangesAsync();

                resp.Value = new DeleteStockResponse { Success = true };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the stock.");
                resp.IsSuccessful = false;
                resp.Error = "An error occurred while deleting the stock.";
                resp.TechMessage = ex.Message;
            }
        }
        return resp;
    }

    public async Task<ServiceResponse<StockWatchListResponse>> AddToWatchlist(int userId, int stockId)
    {
        var resp = new ServiceResponse<StockWatchListResponse>();

        try
        {
            var existingEntry = await _easyStockAppDbContext.WatchLists
                .AnyAsync(w => w.UserId == userId && w.StockId == stockId);

            if (existingEntry)
            {
                resp.IsSuccessful = false;
                resp.Error = "Stock is already in the watchlist.";
                return resp;
            }

            var watchlistEntry = StockWatchList.Create(userId, stockId);
            _easyStockAppDbContext.WatchLists.Add(watchlistEntry);
            await _easyStockAppDbContext.SaveChangesAsync();

            // Fetch the stock details
            var stockDetails = await _easyStockAppDbContext.Stocks
                .Where(s => s.StockId == stockId)
                .Select(s => new
                {
                    s.StockTitle,
                    s.TotalUnits,
                    s.PricePerUnit,
                    s.CompanyName
                })
                .FirstOrDefaultAsync();

            if (stockDetails == null)
            {
                resp.IsSuccessful = false;
                resp.Error = "Stock details not found.";
                return resp;
            }

            resp.Value = new StockWatchListResponse
            {
                WatchlistId = watchlistEntry.WatchlistId,
                UserId = watchlistEntry.UserId,
                StockId = watchlistEntry.StockId,
                StockTitle = stockDetails.StockTitle,
                TotalUnits = stockDetails.TotalUnits.ToString(), // Assuming TotalUnits is numeric
                PricePerUnit = stockDetails.PricePerUnit,
                CompanyName = stockDetails.CompanyName
            };
            resp.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the watchlist.");
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while retrieving the watchlist.";
            resp.TechMessage = ex.Message;
        }
        return resp;
    }

    public async Task<ServiceResponse<StockWatchListResponse>> RemoveFromWatchList(int userId, int stockId)
    {
        var resp = new ServiceResponse<StockWatchListResponse>();

        try
        {
            var watchlistEntry = await _easyStockAppDbContext.WatchLists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.StockId == stockId);

            if (watchlistEntry == null)
            {
                resp.IsSuccessful = false;
                resp.Error = "The stock is not in the watchlist.";
                return resp;
            }

            _easyStockAppDbContext.WatchLists.Remove(watchlistEntry);
            await _easyStockAppDbContext.SaveChangesAsync();

            var stockDetails = await _easyStockAppDbContext.Stocks
                .Where(s => s.StockId == stockId)
                .Select(s => new
                {
                    s.StockTitle
                })
                .FirstOrDefaultAsync();

            resp.IsSuccessful = true;
            resp.Value = new StockWatchListResponse
            {
                WatchlistId = watchlistEntry.WatchlistId,
                UserId = watchlistEntry.UserId,
                StockId = watchlistEntry.StockId,
                StockTitle = stockDetails?.StockTitle ?? "Unknown" // Handle case where stock details might not be available
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing from the watchlist.");
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while removing from the watchlist.";
            resp.TechMessage = ex.Message;
        }
        return resp;
    }

    public async Task<ServiceResponse<GetWatchList>> GetWatchlist(int userId)
    {
        var resp = new ServiceResponse<GetWatchList>();

        try
        {
            var watchlist = await _easyStockAppDbContext.WatchLists
                .Where(w => w.UserId == userId)
                .Select(w => new StockWatchListResponse
                {
                    WatchlistId = w.WatchlistId,
                    UserId = w.UserId,
                    StockId = w.StockId,
                    StockTitle = w.Stock.StockTitle,
                    CompanyName = w.Stock.CompanyName
                }).ToListAsync();

            resp.Value = new GetWatchList { WatchLists = watchlist };
            resp.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all stocks.");
            resp.IsSuccessful = false;
            resp.Error = "An error occurred while fetching stocks.";
            resp.TechMessage = ex.Message;
        }

        return resp;
    }

    //Helper Methods

    private Stocks CreateStockEntity(CreateStockRequest request)
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

    private async Task UpdateStockEntity(Stocks existingStock, UpdateStockRequest request)
    {
        existingStock.Update(
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