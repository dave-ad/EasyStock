namespace EasyStocks.DTO.Requests;

public class CreateFreelanceBrokerRequest
{
    public List<UserRequest> Users { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
}