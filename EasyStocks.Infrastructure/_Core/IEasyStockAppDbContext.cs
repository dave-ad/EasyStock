namespace EasyStocks.Infrastructure;

public interface IEasyStockAppDbContext
{
    DbSet<Broker> Brokers { get; set; }
    //DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}