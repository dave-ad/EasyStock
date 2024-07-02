namespace EasyStocks.DTO.Requests;

public class CreateBrokerRequest
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;
    public string CompanyMobileNumber { get; set; } = string.Empty;
    // Address Property
    public string StreetNo { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string CACRegistrationNumber { get; set; } = string.Empty;
    public string StockBrokerLicenseNumber { get; set; } = string.Empty;
    public DateTime DateCertified { get; set; }
    public string PositionInOrg { get; set; } = string.Empty!;
    public DateOnly DateOfEmployment { get; set; }

    public string BusinessAddress { get; set; } = string.Empty;
    public string ProfessionalQualification { get; set; } = string.Empty;
}