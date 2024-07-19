namespace EasyStocks.Domain.Entities;

public class EasyStockUser : User
{
    public DateOnly? DateOfBirth { get; private set; }
    public Address? Address { get; private set; } = Address.Default();
    public NIN? NIN { get; private set; } = NIN.Default();

    public AccountStatus? Status { get; private set; }
    public List<Transactions>? Transactions { get; set; } = new List<Transactions>();

    private EasyStockUser() { }

    public EasyStockUser(FullName name, string email, MobileNo mobileNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin, AccountStatus? status)
         : base(name, email, mobileNumber, gender)
    {
        DateOfBirth = dateOfBirth;
        Address = address;
        NIN = nin;
        Status = status;
    }

    public static EasyStockUser Create(FullName name, string email, MobileNo mobileNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin)
    {
        return new EasyStockUser(name, email, mobileNumber, gender, dateOfBirth, address, nin, AccountStatus.Pending);
    }

    public void AddTransaction(Transactions transaction)
    {
        Transactions.Add(transaction);
    }
}