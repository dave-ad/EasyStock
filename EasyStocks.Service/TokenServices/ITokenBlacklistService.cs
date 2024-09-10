namespace EasyStocks.Service.TokenServices;

public interface ITokenBlacklistService
{
    Task<bool> IsTokenBlacklistedAsync(string token);
    Task<bool> BlacklistTokenAsync(string token);
}