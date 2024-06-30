namespace EasyStocks.Infrastructure;

public class EasyStockAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IEasyStockAppDbContext
{
    public EasyStockAppDbContext(DbContextOptions<EasyStockAppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CorporateBroker>().HasBaseType<User>();
        builder.Entity<IndividualBroker>().HasBaseType<User>();
        builder.Entity<FreelanceBroker>().HasBaseType<User>();
    }

    public DbSet<CorporateBroker> CorporateBrokers { get; set; }
    public DbSet<IndividualBroker> IndividualBrokers { get; set; }
    public DbSet<FreelanceBroker> FreelanceBrokers { get; set; }
}