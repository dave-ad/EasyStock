namespace EasyStocks.DTO.Responses;

public class StockResponse : IServiceResponse
{
    public int Id { get; set; }
    public string StockTitle { get; set; }
    public string CompanyName { get; set; }
    public string StockType { get; set; }
    public string TotalUnits { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string MinimumPurchase { get; set; }
    public decimal InitialDeposit { get; set; }
    public DateTime DateListed { get; set; }
    public string ListedBy { get; set; }
}