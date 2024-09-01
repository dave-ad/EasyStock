namespace EasyStocks.DTO.Responses;

public class TransactionLog
{
    public string TransactionId { get; set; }
    public string UserId { get; set; }
    public int StockId { get; set; }
    public string UnitPurchase { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
}