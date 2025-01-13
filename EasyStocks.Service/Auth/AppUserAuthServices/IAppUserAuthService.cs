namespace EasyStocks.Service.UserAuthServices;

public interface IAppUserAuthService
{
    Task<ServiceResponse<RegisterResponse>> RegisterAppUserAsync(RegisterUserRequest request);
    Task<ServiceResponse<LoginResponse>> LoginAppUserAsync(LoginUserRequest request);
    Task<ServiceResponse<LogoutResponse>> LogoutAppUserAsync(LogoutRequest request);
}