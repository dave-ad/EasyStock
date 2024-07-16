namespace EasyStocks.DTO.Requests;

public class UpdateFreelanceBrokerRequest
{
    public int BrokerId { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
}