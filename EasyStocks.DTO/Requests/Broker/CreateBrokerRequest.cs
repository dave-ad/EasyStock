namespace EasyStocks.DTO.Requests;

public class CreateBrokerRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string StreetNo { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string NIN { get; set; } = string.Empty;

    // Broker's professional details
    public string BrokerLicense { get; set; } = string.Empty; 
    public DateOnly? DateCertified { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
    public string BrokerType { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";

    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

}