namespace EasyStocks.DTO.Requests;

public class CreateAdminRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int SuperAdminLevel { get; set; }
    public List<string>? Permissions { get; set; } = new List<string>();

    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}