using EasyStocks.Infrastructure.Validator;

namespace EasyStocks.Infrastructure;

public static class DIRegister
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEasyStockAppDbContext, EasyStockAppDbContext>();
        services.AddScoped<BrokerValidator>();
    }
}