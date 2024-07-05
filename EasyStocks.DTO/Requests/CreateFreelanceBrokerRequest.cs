namespace EasyStocks.DTO.Requests;

public class CreateFreelanceBrokerRequest
{
    //public string FirstName { get; set; } = string.Empty;
    //public string LastName { get; set; } = string.Empty;
    //public string OtherNames { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    //public string MobileNumber { get; set; } = string.Empty;
    //public Gender Gender { get; set; }

    // Additional properties for handling users
    public List<UserRequest> Users { get; set; }
    public string ProfessionalQualification { get; set; } = string.Empty;
}