namespace EasyStocks.DTO.Responses;

internal class TransactionResult
{
    public string TransactionId { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
}