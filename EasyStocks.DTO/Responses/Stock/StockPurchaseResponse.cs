namespace EasyStocks.DTO.Responses;

public class StockPurchaseResponse : IServiceResponse
{
    public int TransactionId { get; set; }
    public int StockId { get; set; }
    public string UnitPurchase { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
}