namespace EasyStocks.Service.AuthServices;

public interface IAuthService
{
    Task<IdentityResult> CreateAdminAsync(CreateAdminRequest request);
    Task<SignInResult> LoginAdminAsync(string email, string password);
    Task<SignInResult> LoginBrokerAdminAsync(string email, string password);
    Task<IdentityResult> CreateEasyStockUserAsync(RegisterEasyStockUserRequest request);
    Task<SignInResult> LoginEasyStockUserAsync(string email, string password);
    Task LogoutAsync();
}