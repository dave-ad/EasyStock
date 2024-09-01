namespace EasyStocks.DTO.Responses;

public class WatchlistItemResponse
{
    public int WatchlistId { get; set; }
    public int StockId { get; set; }
    public string StockTitle { get; set; }
    public string TotalUnits { get; set; }
    public decimal PricePerUnit { get; set; }
}