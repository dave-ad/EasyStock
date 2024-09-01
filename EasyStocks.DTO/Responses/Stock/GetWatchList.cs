namespace EasyStocks.DTO.Responses;

public class GetWatchList : IServiceResponse
{
    public List<StockWatchListResponse> WatchLists { get; set; } = new List<StockWatchListResponse>();
}