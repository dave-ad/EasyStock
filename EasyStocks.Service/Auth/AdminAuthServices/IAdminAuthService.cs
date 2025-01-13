namespace EasyStocks.Service.AdminAuthServices;

public interface IAdminAuthService
{
    Task<ServiceResponse<RegisterResponse>> CreateAdminAsync(CreateAdminRequest request);
    Task<ServiceResponse<LoginResponse>> LoginAdminAsync(LoginAdminRequest request);
    Task<ServiceResponse<LogoutResponse>> LogoutAdminAsync(LogoutRequest request);
}