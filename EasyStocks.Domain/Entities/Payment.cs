namespace EasyStocks.Domain.Entities;

public class Payment : Entity
{
    public int PaymentId { get; private set; }
    public int TransactionId { get; private set; }
    public Transaction Transaction { get; private set; } = default!;
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }

    private Payment() { }
    private Payment(int transactionId, decimal amount, DateTime paymentDate, PaymentStatus status)
    {
        TransactionId = transactionId;
        Amount = amount;
        PaymentDate = paymentDate;
        Status = status;
    }

    public static Payment Create(int transactionId, decimal amount, DateTime paymentDate)
    {
        return new Payment(transactionId, amount, paymentDate, PaymentStatus.Pending);
    }

    public void UpdateStatus(PaymentStatus newStatus)
    {
        Status = newStatus;
    }
}
