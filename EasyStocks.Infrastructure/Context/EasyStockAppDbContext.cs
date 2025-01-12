namespace EasyStocks.Infrastructure.Context;

internal sealed class EasyStockAppDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IEasyStockAppDbContext
{
    internal const string DEFAULT_SCHEMA = "ThriftSchema";

    public EasyStockAppDbContext(DbContextOptions<EasyStockAppDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(DEFAULT_SCHEMA);
        builder.ApplyConfigurationsFromAssembly(typeof(EasyStockAppDbContext).Assembly);

        builder.ApplyConfiguration(new UserConfig());
        builder.ApplyConfiguration(new AdminConfig());
        builder.ApplyConfiguration(new AppUserConfig());
        builder.ApplyConfiguration(new BrokerConfig());
        builder.ApplyConfiguration(new StockConfig());
        builder.ApplyConfiguration(new TransactionConfig());
    }

    public DbSet<Broker> Brokers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    //public DbSet<User> Users { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<StockWatchList> WatchLists { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
}