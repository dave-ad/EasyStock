namespace EasyStocks.Domain.Entities;

public class Transaction : Entity
{
    public int TransactionId { get; set; }

    public int StockId { get; private set; }
    public Stock Stock { get; private set; } = default!;
    public int UserId { get; private set; }
    public AppUser User { get; private set; } = default!;
    public decimal PricePerUnit { get; private set; }
    public string UnitPurchase { get; private set; }
    public decimal TransactionAmount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public TransactionStatus Status { get; private set; }

    private Transaction() { }
    private Transaction(int stockId, int userId, decimal pricePerUnit, string unitPurchase, decimal transactionAmount, DateTime transactionDate, TransactionStatus status)
    {
        StockId = stockId;
        UserId = userId;
        PricePerUnit = pricePerUnit;
        UnitPurchase = unitPurchase;
        TransactionAmount = transactionAmount;
        TransactionDate = transactionDate;
        Status = status;

        ValidateTransactionAmount();
    }

    public static Transaction Create(int stockId, int userId, decimal pricePerUnit, string unitPurchase, DateTime transactionDate)
    {
        var transactionAmount = CalculateTransactionAmount(pricePerUnit, unitPurchase);
        return new Transaction(stockId, userId, pricePerUnit, unitPurchase, transactionAmount, transactionDate, TransactionStatus.Pending);
    }

    // Method to calculate transaction amount
    private static decimal CalculateTransactionAmount(decimal pricePerUnit, string unitPurchase)
    {
        if (decimal.TryParse(unitPurchase, out var units) && units >= 0)
        {
            return pricePerUnit * units;
        }
        throw new ArgumentException("UnitPurchase must be a valid non-negative number.");
    }

    // Method to validate transaction amount
    private void ValidateTransactionAmount()
    {
        var expectedAmount = CalculateTransactionAmount(PricePerUnit, UnitPurchase);
        if (TransactionAmount != expectedAmount)
        {
            throw new InvalidOperationException("TransactionAmount does not match the expected amount based on PricePerUnit and UnitPurchase.");
        }
    }
}