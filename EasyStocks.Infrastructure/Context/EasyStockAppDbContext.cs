namespace EasyStocks.Infrastructure;

//public class EasyStockAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IEasyStockAppDbContext
public class EasyStockAppDbContext : DbContext, IEasyStockAppDbContext
{
    public EasyStockAppDbContext(DbContextOptions<EasyStockAppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BrokerConfig());
        //builder.ApplyConfiguration(new ApplicationUserConfig());
    }

    public DbSet<Broker> Brokers { get; set; }
}