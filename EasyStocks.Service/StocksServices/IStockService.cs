namespace EasyStocks.Service.StocksServices;

public interface IStockService
{
    Task<ServiceResponse<StockListResponse>> GetAllStocks();
    Task<ServiceResponse<StockIdResponse>> CreateStock(CreateStockRequest request);
    Task<ServiceResponse<StockResponse>> GetStockById(int stockId);
    Task<ServiceResponse<DeleteStockResponse>> DeleteStock(int stockId);
}