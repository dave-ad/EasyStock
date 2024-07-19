namespace EasyStocks.Domain.Entities;

public class BrokerAdmin : User
{
    // Broker Specific Properties
    public string? PositionInOrg { get; private set; } = default!;
    public DateOnly? DateOfEmployment { get; private set; }
    public AccountStatus? Status { get; private set; }

    public int BrokerId { get; set; }
    public Broker Broker { get; set; }

    private BrokerAdmin() { }
    public BrokerAdmin(FullName name, string email, MobileNo mobileNumber, Gender gender, string? positionInOrg, DateOnly? dateOfEmployment, AccountStatus status)
        : base(name, email, mobileNumber, gender)
    {
        PositionInOrg = positionInOrg;
        DateOfEmployment = dateOfEmployment;
        Status = status;
    }

    // Methods for creating and updating a broker user  
    public static BrokerAdmin Create(FullName name, string email, MobileNo mobileNumber, Gender gender, string? positionInOrg, DateOnly? dateOfEmployment)
    {
        return new BrokerAdmin(name, email, mobileNumber, gender, positionInOrg, dateOfEmployment, AccountStatus.Pending);
    }

    public void Update(FullName name, string email, MobileNo mobileNumber, Gender gender, string? positionInOrg, DateOnly? dateOfEmployment)
    {
        base.Update(name, email, mobileNumber, gender);
        PositionInOrg = positionInOrg;
        DateOfEmployment = dateOfEmployment;
    }
}