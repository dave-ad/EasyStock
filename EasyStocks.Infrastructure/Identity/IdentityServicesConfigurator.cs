namespace EasyStocks.Infrastructure.Identity;

public static class IdentityServicesConfigurator
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<EasyStockAppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            // Lockout settings

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        });

        services.ConfigureApplicationCookie(config =>
        {
            // Configure cookie settings
            config.Cookie.Name = "EastStockApp.Cookie";
            // Other cookie settings
        });

        return services;
    }
}