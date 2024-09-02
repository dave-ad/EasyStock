using EasyStocks.Service.AdminServices;
using EasyStocks.Service.TokenServices;

namespace EasyStocks.Service;

public static class DIRegister
{
    public static IServiceCollection AddEasyStockServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IBrokerService, BrokerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IStockService, StockService>();
        return services;
    }
}