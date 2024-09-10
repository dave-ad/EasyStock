namespace EasyStocks.Domain.Entities;

public partial class Broker : Entity
{
    public int BrokerId { get; set; }
    public List<BrokerAdmin> Users { get; private set; } = default!;

    // CorporateBroker
    public string? CompanyName { get; private set; } = default!;
    public Email? CompanyEmail { get; private set; } = Email.Default();
    public MobileNo? CompanyMobileNumber { get; private set; } = MobileNo.Default();
    public Address? CompanyAddress { get; private set; } = Address.Default();
    public CAC? CACRegistrationNumber { get; private set; } = CAC.Default();
    public StockBrokerLicense? StockBrokerLicense { get; private set; } = StockBrokerLicense.Default();
    public DateOnly? DateCertified { get; private set; }

    // Individual Brokers
    public Address? BusinessAddress { get; private set; } = Address.Default();

    // Freelance Brokers
    public string? ProfessionalQualification { get; private set; } = default!;

    public BrokerRole BrokerType { get; private set; }
    public AccountStatus Status { get; private set; }
}