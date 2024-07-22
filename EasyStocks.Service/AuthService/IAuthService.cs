namespace EasyStocks.Service.AuthServices;

public interface IAuthService
{
    //Task<IdentityResult> RegisterEasyStockUserAsync(RegisterEasyStockUserRequest request);
    Task<SignInResult> LoginBrokerAdminAsync(string email, string password);
    Task LogoutAsync();
}