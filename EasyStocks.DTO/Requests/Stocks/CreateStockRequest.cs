namespace EasyStocks.DTO.Requests;

public class CreateStockRequest
{
    public string StockTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string StockType { get; set; } = string.Empty;
    public string TotalUnits { get; set; } = string.Empty;
    public decimal PricePerUnit { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string MinimumPurchase { get; set; } = string.Empty;
    public decimal InitialDeposit { get; set; }
    public DateTime DateListed { get; set; }
    public string ListedBy { get; set; } = string.Empty;
}