namespace EasyStocks.Infrastructure.Context;

internal sealed class EasyStockAppDbContext : IdentityDbContext<User>, IEasyStockAppDbContext
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

        builder.ApplyConfiguration(new CorporateBrokerConfig());
        builder.ApplyConfiguration(new IndividualBrokerConfig());
        builder.ApplyConfiguration(new FreelanceBrokerConfig());
        builder.ApplyConfiguration(new UserConfig());
    }

    public DbSet<Broker> Brokers { get; set; }
    //public DbSet<User> Users { get; set; }
}