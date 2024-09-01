namespace EasyStocks.Domain.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public int UserId { get; set; }
    public int StockId { get; set; }
    public decimal Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string Status { get; set; }
}
