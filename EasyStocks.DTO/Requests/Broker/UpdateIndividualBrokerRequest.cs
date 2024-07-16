namespace EasyStocks.DTO.Requests;

public class UpdateIndividualBrokerRequest
{
    public int BrokerId { get; set; }
    // Address Property
    public string StreetNo { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string ProfessionalQualification { get; set; } = string.Empty;
    public string StockBrokerLicense { get; set; } = string.Empty;

    public DateOnly? DateCertified { get; set; }
}