namespace EasyStocks.DTO.Requests;

public class UpdateStockRequest
{
    public int Id { get; set; }
    public string StockTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string StockType { get; set; } = string.Empty;
    public string TotalUnits { get; set; } = string.Empty;
    public decimal PricePerUnit { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string MinimumPurchase { get; set; } = string.Empty;
    public DateTime DateListed { get; set; }
    public string ListedBy { get; set; } = string.Empty;
}