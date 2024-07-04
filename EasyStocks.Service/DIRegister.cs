namespace EasyStocks.Service;

public static class DIRegister
{
    public static IServiceCollection AddEasyStockServices(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddScoped<IBrokerService, BrokerService>();
        return services;
    }
}