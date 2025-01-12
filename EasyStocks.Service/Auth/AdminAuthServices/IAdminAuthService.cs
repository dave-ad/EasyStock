namespace EasyStocks.Service.AdminAuthServices;

public interface IAdminAuthService
{
    Task<RegisterResponse> CreateAdminAsync(CreateAdminRequest request);
    Task<LoginResponse> LoginAdminAsync(LoginAdminRequest request);
    Task<LogoutResponse> LogoutAdminAsync(LogoutRequest request);
}