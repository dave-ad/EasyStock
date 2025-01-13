namespace EasyStocks.DTO.Responses;

public class LogoutResponse : IServiceResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> Errors { get; set; }
}