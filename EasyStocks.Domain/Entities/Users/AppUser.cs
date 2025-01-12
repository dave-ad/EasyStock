namespace EasyStocks.Domain.Entities;

public class AppUser : User
{
    public DateOnly? DateOfBirth { get; private set; }
    public Address? Address { get; private set; } = Address.Default();
    public NIN? NIN { get; private set; } = NIN.Default();
    public AccountStatus? Status { get; private set; }

    public List<StockWatchList> Watchlists { get; set; } = new List<StockWatchList>();
    public List<Transaction>? Transactions { get; set; } = new List<Transaction>();

    private AppUser() { }

    public AppUser(FullName name, string email, string phoneNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin, AccountStatus? status)
         : base(name, email, phoneNumber, gender, address, nin)
    {
        DateOfBirth = dateOfBirth;
        Address = address;
        NIN = nin;
        Status = status;
    }

    public static AppUser Create(FullName name, string email, string phoneNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin)
    {
        return new AppUser(name, email, phoneNumber, gender, dateOfBirth, address, nin, AccountStatus.Pending);
    }

    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
}