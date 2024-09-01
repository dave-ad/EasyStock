namespace EasyStocks.Service.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    public TransactionService(IEasyStockAppDbContext easyStockAppDbContext)
    {
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
    }
}