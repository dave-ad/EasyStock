namespace EasyStocks.Service.AdminServices;

public interface IAdminAuthService
{
    Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request);
    Task<LoginResponse> LoginAdminAsync(LoginAdminRequest request);
}