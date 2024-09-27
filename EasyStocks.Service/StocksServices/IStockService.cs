namespace EasyStocks.Service.StocksServices;

public interface IStockService
{
    Task<ServiceResponse<StockListResponse>> GetAll(QueryObject query);
    Task<ServiceResponse<StockResponse>> Create(CreateStockRequest request);
    Task<ServiceResponse<StockResponse>> GetStockById(int stockId);
    Task<ServiceResponse<StockResponse>> UpdateStock(UpdateStockRequest request);
    Task<ServiceResponse<DeleteStockResponse>> DeleteStock(int stockId);

    //Task<ServiceResponse<StockWatchListResponse>> AddToWatchlist(int userId, int stockId);
    //Task<ServiceResponse<StockWatchListResponse>> RemoveFromWatchList(int userId, int stockId);
    //Task<ServiceResponse<GetWatchList>> GetWatchlist(int userId);

    //Task<ServiceResponse<StockPurchaseResponse>> BuyStock(BuyStockRequest request);
}