namespace EasyStocks.Service.AdminAuthServices;

public interface IAdminAuthService
{
    Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request);
    Task<LoginResponse> LoginAdminAsync(LoginAdminRequest request);
    Task<LogoutResponse> LogoutAdminAsync(string token);
}