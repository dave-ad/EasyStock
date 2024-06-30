namespace EasyStocks.Domain.Enitities;

public partial class FreelanceBroker : User, IAggregateRoot
{
    public int FreelanceBrokerid { get; set; }
    public string ProfessionalQualification { get; private set; } = default!;
}