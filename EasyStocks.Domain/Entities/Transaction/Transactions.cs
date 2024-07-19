namespace EasyStocks.Domain.Entities;

public class Transactions : Entity
{
    public int TransactionId { get; set; }

    // Relationships
    public int Id { get; private set; }
    public EasyStockUser User { get; private set; } = default!;
    public int StockId { get; private set; }
    public Stocks Stock { get; private set; } = default!;

    public decimal TransactionAmount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }

    private Transactions() { }
    private Transactions(int id, int stockId, decimal transactionAmount, DateTime transactionDate, TransactionType type, TransactionStatus status)
    {
        Id = id;
        StockId = stockId;
        TransactionAmount = transactionAmount;
        TransactionDate = transactionDate;
        Type = type;
        Status = status;
    }

    public static Transactions Create(int id, int stockId, decimal transactionAmount, DateTime transactionDate, TransactionType type)
    {
        return new Transactions(id, stockId, transactionAmount, transactionDate, type, TransactionStatus.Pending);
    }

    public void UpdateStatus(TransactionStatus newStatus)
    {
        Status = newStatus;
    }
}