namespace EasyStocks.DTO.Requests;

public class BuyStockRequest
{
    public int UserId { get; set; }
    public int StockId { get; set; }
    public string UnitPurchase { get; set; }

    // Additional properties if needed
    public string StockTitle { get; set; }
    public string CompanyName { get; set; }
    public decimal PricePerUnit { get; set; }
    public string TotalUnits { get; set; }
}