namespace EasyStocks.DTO.Requests;

public class BrokerAdminRequest
{
    public int BrokerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string? PositionInOrg { get; set; } = string.Empty;
    public DateOnly? DateOfEmployment { get; set; }
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}