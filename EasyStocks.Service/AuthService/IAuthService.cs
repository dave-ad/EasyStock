namespace EasyStocks.Service.AuthService;

public interface IAuthService
{
    //Task<IdentityResult> RegisterUserAsync(UserRequest userRequest);
    Task<SignInResult> LoginUserAsync(string email, string password);
}