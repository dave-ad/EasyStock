namespace EasyStocks.Infrastructure;

public interface IEasyStockAppDbContext
{
    DbSet<Broker> Brokers { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}