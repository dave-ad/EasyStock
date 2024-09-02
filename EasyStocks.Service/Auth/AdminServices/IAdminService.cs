namespace EasyStocks.Service.AdminServices;

public interface IAdminService
{
    Task<RegisterResponse> RegisterAdminAsync(RegisterAdminRequest request);
    Task<LoginResponse> LoginAdminAsync(LoginAdminRequest request);
}