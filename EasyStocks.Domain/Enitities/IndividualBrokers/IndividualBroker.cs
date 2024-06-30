namespace EasyStocks.Domain.Enitities;

public partial class IndividualBroker : User, IAggregateRoot
{
    public int IndividualBrokerId { get; set; }
    public Address BusinessAddress { get; private set; } = Address.Default();
    public StockBrokerLicense StockBrokerLicenseNumber { get; private set; } = StockBrokerLicense.Default();
    public DateTime DateCertified { get; private set; }
    public string ProfessionalQualification { get; private set; } = default!;
    //public CorporateBroker CorporateBroker { get; private set; } // If registered under a corporate broker
}
