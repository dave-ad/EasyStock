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
        builder.ApplyConfiguration(new BrokerAdminConfig());
        builder.ApplyConfiguration(new EasyStockUserConfig());
        builder.ApplyConfiguration(new CorporateBrokerConfig());
        builder.ApplyConfiguration(new IndividualBrokerConfig());
        builder.ApplyConfiguration(new FreelanceBrokerConfig());
        builder.ApplyConfiguration(new StocksConfig());
    }

    public DbSet<Broker> Brokers { get; set; }
    public DbSet<Stocks> Stocks { get; set; }
    public DbSet<Transactions> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<BrokerAdmin> BrokerAdmins { get; set; }
    public DbSet<EasyStockUser> EasyStockUsers { get; set; }
}