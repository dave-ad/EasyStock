namespace EasyStocks.DTO.Responses;

public class LoginResponse : IServiceResponse
{
{
    public bool Success { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> Errors { get; set; } // If any
}