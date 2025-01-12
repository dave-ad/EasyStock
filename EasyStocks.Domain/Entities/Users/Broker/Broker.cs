namespace EasyStocks.Domain.Entities;

public partial class Broker : User
{
    // Brokage info
    public BrokerLicense? BrokerLicense { get; private set; } = BrokerLicense.Default();
    public DateOnly? DateCertified { get; private set; }
    public string? ProfessionalQualification { get; private set; } = default!;
    public BrokerRole BrokerType { get; private set; }
    public AccountStatus Status { get; private set; }
}