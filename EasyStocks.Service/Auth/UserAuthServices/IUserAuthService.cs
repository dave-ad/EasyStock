namespace EasyStocks.Service.UserAuthServices;

public interface IUserAuthService
{
    Task<RegisterResponse> RegisterUserAsync(RegisterUserRequest request);
    Task<LoginResponse> LoginUserAsync(LoginUserRequest request);
}