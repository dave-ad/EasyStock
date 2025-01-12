namespace EasyStocks.DTO.Responses;

public class RegisterResponse : IServiceResponse
{
    public bool Success { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> Errors { get; set; } // If any
}