namespace EasyStocks.Service;

public static class DIRegister
{
    public static IServiceCollection AddEasyStockServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddScoped<IAdminAuthService, AdminAuthService>();
        services.AddScoped<IBrokerAuthService, BrokerAuthService>();
        services.AddScoped<IAppUserAuthService, AppUserAuthService>();
        //services.AddScoped<IBrokerService, BrokerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
        services.AddScoped<IStockService, StockService>();
        return services;
    }
}