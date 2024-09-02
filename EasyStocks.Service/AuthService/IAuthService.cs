using EasyStocks.DTO.Requests;

namespace EasyStocks.Service.AuthServices;

public interface IAuthService
{
    Task<IdentityResult> CreateAdminAsync(RegisterAdminRequest request);
    Task<SignInResult> LoginAdminAsync(string email, string password);
    Task<SignInResult> LoginBrokerAdminAsync(string email, string password);
    Task<IdentityResult> CreateEasyStockUserAsync(RegisterUserRequest request);
    Task<SignInResult> LoginEasyStockUserAsync(string email, string password);
    Task LogoutAsync();
}