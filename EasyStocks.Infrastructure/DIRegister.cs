namespace EasyStocks.Infrastructure;

public static class DIRegister
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEasyStockAppDbContext, EasyStockAppDbContext>();
        services.AddScoped<BrokerValidator>();
        services.AddScoped<StockValidator>();

        // Register DbContext with the service container
        services.AddDbContext<EasyStockAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DevConnection")));

        // Register Identity services
        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<EasyStockAppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        });

        services.ConfigureApplicationCookie(config =>
        {
            // Configure cookie settings
            config.Cookie.Name = "EastStockApp.Cookie";
            // Other cookie settings
        });
    }
}