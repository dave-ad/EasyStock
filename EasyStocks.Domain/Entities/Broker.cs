namespace EasyStocks.Domain.Entities;

public partial class Broker : Entity
{ 
    public int BrokerId { get; set; }
    public FullName Name { get; private set; } = FullName.Default();
    public Email Email { get; private set; } = Email.Default();
    public MobileNo MobileNumber { get; private set; } = MobileNo.Default();
    public Gender Gender { get; private set; }

    // CorporateBroker
    public string? CompanyName { get; private set; } = default!;
    public Email? CompanyEmail { get; private set; } = Email.Default();
    public MobileNo? CompanyMobileNumber { get; private set; } = MobileNo.Default();
    public Address? CompanyAddress { get; private set; } = Address.Default();
    public CAC? CACRegistrationNumber { get; private set; } = CAC.Default();
    public StockBrokerLicense? StockBrokerLicenseNumber { get; private set; } = StockBrokerLicense.Default();
    public DateTime? DateCertified { get; private set; }
    public string? PositionInOrg { get; private set; } = default!;
    public DateOnly? DateOfEmployment { get; private set; }

    // Individual Brokers
    public Address? BusinessAddress { get; private set; } = Address.Default();

    // Freelance Brokers
    public string? ProfessionalQualification { get; private set; } = default!;

    public BrokerType? BrokerType { get; private set; } = default!;
    public AccountStatus? Status { get; private set; }
}