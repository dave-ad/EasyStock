namespace EasyStocks.Domain.Enitities;

public partial class CorporateBroker : User, IAggregateRoot
{
    public int CorporateBrokerId { get; private set; }
    public string CompanyName { get; private set; } = default!;
    public Email CompanyEmail { get; private set; } = Email.Default();
    public MobileNo CompanyMobileNumber { get; private set; } = MobileNo.Default();
    public Address CorporateAddress { get; private set; } = Address.Default();
    public Address CompanyAddress { get; private set; } = Address.Default();
    public CAC CACRegistrationNumber { get; private set; } = CAC.Default();
    public StockBrokerLicense StockBrokerLicenseNumber { get; private set; } = StockBrokerLicense.Default();
    public DateTime DateCertified { get; private set; }
    // Key Staff Info
    public string PositionInOrg { get; private set; } = default!;
    public DateOnly DateOfEmployment { get; private set; }

    public AccountStatus Status { get; private set; }
}