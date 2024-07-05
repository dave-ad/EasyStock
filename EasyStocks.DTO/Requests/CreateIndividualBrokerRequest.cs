namespace EasyStocks.DTO.Requests;

public class CreateIndividualBrokerRequest
{
    public List<UserRequest> Users { get; set; }

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
