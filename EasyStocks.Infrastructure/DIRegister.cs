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

        //// Register Identity services
        //services.AddIdentity<User, IdentityRole<int>>()
        //    .AddEntityFrameworkStores<EasyStockAppDbContext>()
        //    .AddDefaultTokenProviders();
    }
}