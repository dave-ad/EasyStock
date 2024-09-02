namespace EasyStocks.DTO.Requests;

public class LoginAdminRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}