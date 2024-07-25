namespace EasyStocks.DTO.Responses;

public class StockWatchListResponse : IServiceResponse
{
    public int WatchlistId { get; set; }
    public int UserId { get; set; }
    public int StockId { get; set; }
    public string StockTitle { get; set; }
    public string TotalUnits { get; set; }
    public decimal PricePerUnit { get; set; }
    public string CompanyName { get; set; }
}