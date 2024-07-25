namespace EasyStocks.Infrastructure;

public interface IEasyStockAppDbContext
{
    DbSet<Broker> Brokers { get; set; }
    DbSet<Stocks> Stocks { get; set; }
    DbSet<StockWatchList> WatchLists { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}