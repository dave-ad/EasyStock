namespace EasyStocks.Service.TokenServices;

public interface ITokenService
{
    string CreateToken(User user);
}