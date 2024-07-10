using EasyStocks.Service.AuthServices;
using Microsoft.Extensions.Configuration;

namespace EasyStocks.Service;

public static class DIRegister
{
    public static IServiceCollection AddEasyStockServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddScoped<IBrokerService, BrokerService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}