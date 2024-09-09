namespace EasyStocks.DTO.Requests;

public class BrokerLoginRequest
{
    [Required]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}