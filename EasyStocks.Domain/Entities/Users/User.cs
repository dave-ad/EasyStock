namespace EasyStocks.Domain.Entities;

public class User : IdentityUser<int>
{
    public FullName Name { get; protected set; } = FullName.Default();
    public Gender Gender { get; protected set; }
    public Address? Address { get; protected set; } = Address.Default();
    public NIN? NIN { get; protected set; } = NIN.Default();

    protected User() { }
    public User(FullName name, string email, string phoneNumber, Gender gender, Address address, NIN nin)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Address = address;
        NIN = nin;
    }

    public void Update(FullName name, string email, string phoneNumber, Gender gender, Address address, NIN nin)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Address = address;
        NIN = nin;
    }
}