namespace EasyStocks.DTO.Responses;

public class BrokerResponse : IServiceResponse
{
    public int BrokerId { get; set; }
    public List<BrokerAdminResponse> Users { get; set; } = new List<BrokerAdminResponse>();
    public string? CompanyName { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyMobileNumber { get; set; }
    public AddressResponse? CompanyAddress { get; set; }
    public string? CACRegistrationNumber { get; set; }
    public string? StockBrokerLicense { get; set; }
    public DateOnly? DateCertified { get; set; }
    public AddressResponse? BusinessAddress { get; set; }
    public string? ProfessionalQualification { get; set; }
    public BrokerType BrokerType { get; set; }
    public AccountStatus Status { get; set; }
}