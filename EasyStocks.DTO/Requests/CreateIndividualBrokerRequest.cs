namespace EasyStocks.DTO.Requests;

public class CreateIndividualBrokerRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string OtherNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    // Address Property
    public string StreetNo { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string StockBrokerLicenseNumber { get; set; } = string.Empty;
    public DateOnly DateCertified { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
}
