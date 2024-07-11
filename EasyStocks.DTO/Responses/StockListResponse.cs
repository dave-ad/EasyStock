namespace EasyStocks.DTO.Responses;

public class StockListResponse : IServiceResponse
{
    public List<StockResponse> Stocks { get; set; } = new List<StockResponse>();
}