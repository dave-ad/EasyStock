namespace EasyStocks.Infrastructure.Context;

internal sealed class EasyStockAppDbContext : DbContext, IEasyStockAppDbContext
{
    internal const string DEFAULT_SCHEMA = "ThriftSchema";
    private string? DevConnection = "";

    public EasyStockAppDbContext()
    {
        DevConnection = "Server=David\\MSSQLSERVER2022;Database=EasyStockDB;initial catalog=EasyStockDB;Trusted_Connection=True;MultipleActiveResultSets=true;integrated security=True;TrustServerCertificate=True;";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
    .UseSqlServer(DevConnection)
    .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DEFAULT_SCHEMA);
        builder.ApplyConfigurationsFromAssembly(typeof(EasyStockAppDbContext).Assembly);

        builder.ApplyConfiguration(new CorporateBrokerConfig());
        builder.ApplyConfiguration(new IndividualBrokerConfig());
        builder.ApplyConfiguration(new FreelanceBrokerConfig());
    }

    public DbSet<Broker> Brokers { get; set; }
}