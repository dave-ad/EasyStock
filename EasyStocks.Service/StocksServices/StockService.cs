namespace EasyStocks.Service.StocksServices;

public class StockService : IStockService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly StockValidator _validator;
    private readonly ILogger<StockService> _logger;
    public StockService(IEasyStockAppDbContext easyStockAppDbContext, ILogger<StockService> logger, StockValidator validator)
    {
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ServiceResponse<StockResponse>> Create(CreateStockRequest request)
    {
        var resp = new ServiceResponse<StockResponse>();

        var validationResponse = _validator.ValidateStock(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingStock = await _easyStockAppDbContext.Stocks.AnyAsync(s => s.TickerSymbol.Trim().ToUpper() == request.TickerSymbol.ToUpper());

                if (existingStock)
                    return CreateDuplicateErrorResponse(resp, "Stocks");

                var stockEntity = CreateStockEntity(request);
                var addedStock = await _easyStockAppDbContext.Stocks.AddAsync(stockEntity);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (addedStock == null || addedStock?.Entity.StockId < 1)
                    return CreateDatabaseErrorResponse(resp);

                resp.Value = new StockResponse 
                {
                    StockId = addedStock.Entity.StockId,
                    TickerSymbol = addedStock.Entity.TickerSymbol,
                    CompanyName = addedStock.Entity.CompanyName,
                    Exchange = addedStock.Entity.Exchange,

                    // Pricing Information
                    OpeningPrice = addedStock.Entity.OpeningPrice,
                    ClosingPrice = addedStock.Entity.ClosingPrice,
                    CurrentPrice = addedStock.Entity.CurrentPrice,
                    DayHigh = addedStock.Entity.DayHigh,
                    DayLow = addedStock.Entity.DayLow,
                    YearHigh = addedStock.Entity.YearHigh,
                    YearLow = addedStock.Entity.YearLow,

                    // Financial Details
                    OutstandingShares = addedStock.Entity.OutstandingShares,
                    MarketCapitalization = addedStock.Entity.MarketCapitalization,
                    DividendYield = addedStock.Entity.DividendYield,
                    EarningsPerShare = addedStock.Entity.EarningsPerShare,
                    PriceEarningsRatio = addedStock.Entity.PriceEarningsRatio,
                    Volume = addedStock.Entity.Volume,
                    Beta = addedStock.Entity.Beta
                };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
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

    public async Task<ServiceResponse<StockListResponse>> GetAll(QueryObject query)
    {
        var resp = new ServiceResponse<StockListResponse>();

        try
        {
            var stocksQuery = _easyStockAppDbContext.Stocks
                .Select(s => new StockResponse
                {
                    StockId = s.StockId,
                    TickerSymbol = s.TickerSymbol,
                    CompanyName = s.CompanyName,
                    Exchange = s.Exchange,
                    OpeningPrice = s.OpeningPrice,
                    ClosingPrice = s.ClosingPrice,
                    CurrentPrice = s.CurrentPrice,
                    DayHigh = s.DayHigh,
                    DayLow = s.DayLow,
                    YearHigh = s.YearHigh,
                    YearLow = s.YearLow,
                    OutstandingShares = s.OutstandingShares,
                    MarketCapitalization = s.MarketCapitalization,
                    DividendYield = s.DividendYield,
                    EarningsPerShare = s.EarningsPerShare,
                    PriceEarningsRatio = s.PriceEarningsRatio,
                    Volume = s.Volume,
                    Beta = s.Beta
                });

            // Filtering
            if (!string.IsNullOrWhiteSpace(query.TickerSymbol))
            {
                stocksQuery = stocksQuery.Where(s => s.TickerSymbol.Contains(query.TickerSymbol));
            }

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocksQuery = stocksQuery.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocksQuery = query.IsDescending
                ? stocksQuery.OrderByDescending(s => EF.Property<object>(s, query.SortBy))
                : stocksQuery.OrderBy(s => EF.Property<object>(s, query.SortBy));

                //if (query.SortBy.Equals("StockTitle", StringComparison.OrdinalIgnoreCase))
                //{
                //    stocksQuery = query.IsDescending ? stocksQuery.OrderByDescending(s => s.StockTitle) : stocksQuery.OrderBy(s => s.StockTitle);
                //}
                //else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                //{
                //    stocksQuery = query.IsDescending ? stocksQuery.OrderByDescending(s => s.CompanyName) : stocksQuery.OrderBy(s => s.CompanyName);
                //}
            }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var stocks = await stocksQuery.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            resp.Value = new StockListResponse { Stocks = stocks.ToList() };
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
                StockId = stock.StockId,
                TickerSymbol = stock.TickerSymbol,
                CompanyName = stock.CompanyName,
                Exchange = stock.Exchange,
                OpeningPrice = stock.OpeningPrice,
                ClosingPrice = stock.ClosingPrice,
                CurrentPrice = stock.CurrentPrice,
                DayHigh = stock.DayHigh,
                DayLow = stock.DayLow,
                YearHigh = stock.YearHigh,
                YearLow = stock.YearLow,
                OutstandingShares = stock.OutstandingShares,
                MarketCapitalization = stock.MarketCapitalization,
                DividendYield = stock.DividendYield,
                EarningsPerShare = stock.EarningsPerShare,
                PriceEarningsRatio = stock.PriceEarningsRatio,
                Volume = stock.Volume,
                Beta = stock.Beta
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
                var existingStockResponse = await GetStockById(request.StockId);

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
                    TickerSymbol = existingStock.TickerSymbol,
                    CompanyName = existingStock.CompanyName,
                    Exchange = existingStock.Exchange,
                    OpeningPrice = existingStock.OpeningPrice,
                    ClosingPrice = existingStock.ClosingPrice,
                    CurrentPrice = existingStock.CurrentPrice,
                    DayHigh = existingStock.DayHigh,
                    DayLow = existingStock.DayLow,
                    YearHigh = existingStock.YearHigh,
                    YearLow = existingStock.YearLow,
                    OutstandingShares = existingStock.OutstandingShares,
                    DividendYield = existingStock.DividendYield,
                    EarningsPerShare = existingStock.EarningsPerShare,
                    Volume = existingStock.Volume,
                    Beta = existingStock.Beta
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

    //public async Task<ServiceResponse<StockWatchListResponse>> AddToWatchlist(int userId, int stockId)
    //{
    //    var resp = new ServiceResponse<StockWatchListResponse>();

    //    try
    //    {
    //        var existingEntry = await _easyStockAppDbContext.WatchLists
    //            .AnyAsync(w => w.UserId == userId && w.StockId == stockId);

    //        if (existingEntry)
    //        {
    //            resp.IsSuccessful = false;
    //            resp.Error = "Stock is already in the watchlist.";
    //            return resp;
    //        }

    //        var watchlistEntry = StockWatchList.Create(userId, stockId);
    //        _easyStockAppDbContext.WatchLists.Add(watchlistEntry);
    //        await _easyStockAppDbContext.SaveChangesAsync();

    //        // Fetch the stock details
    //        var stockDetails = await _easyStockAppDbContext.Stocks
    //            .Where(s => s.StockId == stockId)
    //            .Select(s => new
    //            {
    //                s.TickerSymbol,
    //                s.OutstandingShares,
    //                s.MarketCapitalization,
    //                s.CompanyName
    //            })
    //            .FirstOrDefaultAsync();

    //        if (stockDetails == null)
    //        {
    //            resp.IsSuccessful = false;
    //            resp.Error = "Stock details not found.";
    //            return resp;
    //        }

    //        resp.Value = new StockWatchListResponse
    //        {
    //            WatchlistId = watchlistEntry.WatchlistId,
    //            UserId = watchlistEntry.UserId,
    //            StockId = watchlistEntry.StockId,
    //            StockTitle = stockDetails.StockTitle,
    //            TotalUnits = stockDetails.TotalUnits.ToString(), // Assuming TotalUnits is numeric
    //            PricePerUnit = stockDetails.PricePerUnit,
    //            CompanyName = stockDetails.CompanyName
    //        };
    //        resp.IsSuccessful = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while retrieving the watchlist.");
    //        resp.IsSuccessful = false;
    //        resp.Error = "An error occurred while retrieving the watchlist.";
    //        resp.TechMessage = ex.Message;
    //    }
    //    return resp;
    //}

    //public async Task<ServiceResponse<StockWatchListResponse>> RemoveFromWatchList(int userId, int stockId)
    //{
    //    var resp = new ServiceResponse<StockWatchListResponse>();

    //    try
    //    {
    //        var watchlistEntry = await _easyStockAppDbContext.WatchLists
    //            .FirstOrDefaultAsync(w => w.UserId == userId && w.StockId == stockId);

    //        if (watchlistEntry == null)
    //        {
    //            resp.IsSuccessful = false;
    //            resp.Error = "The stock is not in the watchlist.";
    //            return resp;
    //        }

    //        _easyStockAppDbContext.WatchLists.Remove(watchlistEntry);
    //        await _easyStockAppDbContext.SaveChangesAsync();

    //        var stockDetails = await _easyStockAppDbContext.Stocks
    //            .Where(s => s.StockId == stockId)
    //            .Select(s => new
    //            {
    //                s.TickerSymbol
    //            })
    //            .FirstOrDefaultAsync();

    //        resp.IsSuccessful = true;
    //        resp.Value = new StockWatchListResponse
    //        {
    //            WatchlistId = watchlistEntry.WatchlistId,
    //            UserId = watchlistEntry.UserId,
    //            StockId = watchlistEntry.StockId,
    //            StockTitle = stockDetails?.StockTitle ?? "Unknown" // Handle case where stock details might not be available
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while removing from the watchlist.");
    //        resp.IsSuccessful = false;
    //        resp.Error = "An error occurred while removing from the watchlist.";
    //        resp.TechMessage = ex.Message;
    //    }
    //    return resp;
    //}

    //public async Task<ServiceResponse<GetWatchList>> GetWatchlist(int userId)
    //{
    //    var resp = new ServiceResponse<GetWatchList>();

    //    try
    //    {
    //        var watchlist = await _easyStockAppDbContext.WatchLists
    //            .Where(w => w.UserId == userId)
    //            .Select(w => new StockWatchListResponse
    //            {
    //                WatchlistId = w.WatchlistId,
    //                UserId = w.UserId,
    //                StockId = w.StockId,
    //                StockTitle = w.Stock.TickerSymbol,
    //                CompanyName = w.Stock.CompanyName
    //            }).ToListAsync();

    //        resp.Value = new GetWatchList { WatchLists = watchlist };
    //        resp.IsSuccessful = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while retrieving all stocks.");
    //        resp.IsSuccessful = false;
    //        resp.Error = "An error occurred while fetching stocks.";
    //        resp.TechMessage = ex.Message;
    //    }

    //    return resp;
    //}

    //public async Task<ServiceResponse<StockPurchaseResponse>> BuyStock(BuyStockRequest request)
    //{
    //    var resp = new ServiceResponse<StockPurchaseResponse>();
    //    try
    //    {
    //        // Fetch stock details
    //        var stock = await _easyStockAppDbContext.Stocks
    //            .FirstOrDefaultAsync(s => s.StockId == request.StockId);

    //        if (stock == null)
    //        {
    //            _logger.LogWarning("Stock not found: {StockId}", request.StockId);
    //            resp.IsSuccessful = false;
    //            resp.Error = "Stock not found.";
    //            return resp;
    //        }

    //        // Fetch user details along with their watchlists
    //        var user = await _easyStockAppDbContext.EasyStockUsers
    //            .Include(u => u.Watchlists) 
    //            .FirstOrDefaultAsync(u => u.Id == request.UserId);

    //        if (user == null)
    //        {
    //            _logger.LogWarning("User not found: {UserId}", request.UserId);
    //            resp.IsSuccessful = false;
    //            resp.Error = "User not found.";
    //            return resp;
    //        }

    //        // Check if the stock is in the user's wishlist
    //        if (!user.Watchlists.Any(w => w.StockId == request.StockId))
    //        {
    //            _logger.LogWarning("Stock not in user's watchlist: {StockId}, UserId: {UserId}", request.StockId, request.UserId);
    //            resp.IsSuccessful = false;
    //            resp.Error = "Stock is not in your wishlist.";
    //            return resp;
    //        }

    //        // Parse and validate the unit purchase amount
    //        if (!decimal.TryParse(request.UnitPurchase, out var unitPurchase) || unitPurchase <= 0)
    //        {
    //            _logger.LogWarning("Invalid unit purchase format: {UnitPurchase}", request.UnitPurchase);
    //            resp.IsSuccessful = false;
    //            resp.Error = "Invalid unit purchase format.";
    //            return resp;
    //        }

    //        var transactionAmount = stock.MarketCapitalization * unitPurchase;
    //        if (transactionAmount < stock.DayHigh)
    //        {
    //            _logger.LogWarning("Insufficient deposit amount: {TransactionAmount}, InitialDeposit: {InitialDeposit}", transactionAmount, stock.DayHigh);
    //            resp.IsSuccessful = false;
    //            resp.Error = "Insufficient deposit amount.";
    //            return resp;
    //        }

    //        // Check if there are enough units available
    //        if ((!decimal.TryParse(stock.OutstandingShares, out var totalUnits) || totalUnits < unitPurchase))
    //        {
    //            _logger.LogWarning("Not enough units available: {AvailableUnits}, RequestedUnits: {RequestedUnits}", stock.OutstandingShares, unitPurchase);
    //            resp.IsSuccessful = false;
    //            resp.Error = "Not enough units available.";
    //            return resp;
    //        }

    //        // Create a new transaction
    //        var transaction = Domain.Entities.Transaction
    //            .Create(request.StockId, 
    //                    request.UserId, 
    //                    stock.MarketCapitalization, 
    //                    request.UnitPurchase, 
    //                    DateTime.Now
    //                    );

    //        // Deduct the purchased units from the stock using the method
    //        stock.UpdateTotalUnits(totalUnits - unitPurchase);

    //        // Create an invoice
    //        var invoice = new Invoice
    //        {
    //            UserId = request.UserId,
    //            StockId = request.StockId,
    //            Quantity = unitPurchase,
    //            PricePerUnit = stock.MarketCapitalization,
    //            TotalAmount = transactionAmount,
    //            InvoiceDate = DateTime.Now,
    //            Status = "Paid"
    //        };

    //        // Add the transaction to the context
    //        _easyStockAppDbContext.Transactions.Add(transaction);
    //        _easyStockAppDbContext.Invoices.Add(invoice);
    //        user.AddTransaction(transaction);
    //        stock.Transactions.Add(transaction);

    //        // Save changes to the database
    //        await _easyStockAppDbContext.SaveChangesAsync();

    //        // Log the successful transaction
    //        _logger.LogInformation("Transaction successful: {TransactionId}, UserId: {UserId}, StockId: {StockId}, Amount: {TotalAmount}", transaction.TransactionId, request.UserId, request.StockId, transactionAmount);

    //        // Set the response value
    //        resp.Value = new StockPurchaseResponse
    //        {
    //            TransactionId = transaction.TransactionId,
    //            StockId = stock.StockId,
    //            UnitPurchase = request.UnitPurchase,
    //            PricePerUnit = stock.MarketCapitalization,
    //            TotalAmount = transactionAmount,
    //            TransactionDate = transaction.TransactionDate,
    //            Status = transaction.Status
    //        };
    //        resp.IsSuccessful = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while processing the transaction for UserId: {UserId}, StockId: {StockId}", request.UserId, request.StockId);
    //        resp.IsSuccessful = false;
    //        resp.Error = "An error occurred while processing the transaction.";
    //        resp.TechMessage = ex.Message;
    //    }

    //    return resp;
    //}

    //Helper Methods

    private Stock CreateStockEntity(CreateStockRequest request)
    {
        var stock = Stock.Create(
        request.TickerSymbol,
        request.CompanyName,
        request.Exchange,
        request.OpeningPrice,
        request.ClosingPrice,
        request.CurrentPrice,
        request.DayHigh,
        request.DayLow,
        request.YearHigh,
        request.YearLow,
        request.OutstandingShares,
        request.DividendYield,
        request.EarningsPerShare,
        request.Volume,
        request.Beta
        );

        return stock;
    }

    private async Task UpdateStockEntity(Stock existingStock, UpdateStockRequest request)
    {
        existingStock.Update(
            request.TickerSymbol,
            request.CompanyName,
            request.Exchange,
            request.OpeningPrice,
            request.ClosingPrice,
            request.CurrentPrice,
            request.DayHigh,
            request.DayLow,
            request.YearHigh,
            request.YearLow,
            request.OutstandingShares,
            request.DividendYield,
            request.EarningsPerShare,
            request.Volume,
            request.Beta
        );
    }

    private static ServiceResponse<StockResponse> CreateDuplicateErrorResponse(ServiceResponse<StockResponse> resp, string entityType)
    {
        resp.Error = $"{entityType} with the provided details already exists.";
        resp.TechMessage = $"Duplicate Error. A {entityType} with the provided details already exists in the database.";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<StockResponse> CreateDatabaseErrorResponse(ServiceResponse<StockResponse> resp, Exception ex = null)
    {
        resp.Error = "An unexpected error occurred while processing your request.";
        resp.TechMessage = ex == null ? "Unknown Database Error" : $"Database Error: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<StockResponse> CreateExceptionResponse(ServiceResponse<StockResponse> resp, Exception ex)
    {
        resp.Error = "An unexpected error occurred. Please try again later.";
        resp.TechMessage = $"Exception: {ex.GetBaseException().Message}";
        resp.IsSuccessful = false;
        return resp;
    }
}