namespace EasyStocks.Domain.Entities;

public class EasyStockUser : User
{
    public DateOnly? DateOfBirth { get; private set; }
    public Address? Address { get; private set; } = Address.Default();
    public NIN? NIN { get; private set; } = NIN.Default();
    public AccountStatus? Status { get; private set; }

    public List<StockWatchList> Watchlists { get; set; } = new List<StockWatchList>();
    public List<Transaction>? Transactions { get; set; } = new List<Transaction>();

    private EasyStockUser() { }

    public EasyStockUser(FullName name, string email, string phoneNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin, AccountStatus? status)
         : base(name, email, phoneNumber, gender)
    {
        DateOfBirth = dateOfBirth;
        Address = address;
        NIN = nin;
        Status = status;
    }

    public static EasyStockUser Create(FullName name, string email, string phoneNumber, Gender gender, DateOnly? dateOfBirth, Address? address, NIN? nin)
    {
        return new EasyStockUser(name, email, phoneNumber, gender, dateOfBirth, address, nin, AccountStatus.Pending);
    }

    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
}