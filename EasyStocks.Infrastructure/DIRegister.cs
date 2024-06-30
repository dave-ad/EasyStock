
namespace EasyStocks.Infrastructure;

public static class DIRegister
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEasyStockAppDbContext, EasyStockAppDbContext>();
    }
}