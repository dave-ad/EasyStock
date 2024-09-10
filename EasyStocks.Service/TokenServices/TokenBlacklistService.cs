namespace EasyStocks.Service.TokenServices;

internal class TokenBlacklistService : ITokenBlacklistService
{
    // In-memory store for blacklisted tokens
    private readonly ConcurrentDictionary<string, bool> _blacklistedTokens = new ConcurrentDictionary<string, bool>();

    public Task<bool> BlacklistTokenAsync(string token)
    {
        // Attempt to add the token to the blacklist
        // Returns true if the token was successfully added, false if it was already present
        bool result = _blacklistedTokens.TryAdd(token, true);
        return Task.FromResult(result);
    }

    public Task<bool> IsTokenBlacklistedAsync(string token)
    {
        // Returns true if the token is found in the blacklist, otherwise false
        bool result = _blacklistedTokens.ContainsKey(token);
        return Task.FromResult(result);
    }
}