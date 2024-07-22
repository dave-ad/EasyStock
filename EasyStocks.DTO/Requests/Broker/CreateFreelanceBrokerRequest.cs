namespace EasyStocks.DTO.Requests;

public class CreateFreelanceBrokerRequest
{
    public List<BrokerAdminRequest> Users { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
}