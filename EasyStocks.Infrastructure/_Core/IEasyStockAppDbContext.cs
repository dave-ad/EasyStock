namespace EasyStocks.Infrastructure;

public interface IEasyStockAppDbContext
{
    DbSet<Broker> Brokers { get; set; }
    DbSet<Stock> Stocks { get; set; }
    DbSet<StockWatchList> WatchLists { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    //DbSet<User> Users { get; set; }
    DbSet<Admin> Admins { get; set; }
    DbSet<AppUser> AppUsers { get; set; }
    DbSet<Invoice> Invoices { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}