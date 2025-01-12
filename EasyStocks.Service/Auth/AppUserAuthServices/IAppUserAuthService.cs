namespace EasyStocks.Service.UserAuthServices;

public interface IAppUserAuthService
{
    Task<RegisterResponse> RegisterUserAsync(RegisterUserRequest request);
    Task<LoginResponse> LoginUserAsync(LoginUserRequest request);
    Task<LogoutResponse> LogoutUserAsync(LogoutRequest request);
}