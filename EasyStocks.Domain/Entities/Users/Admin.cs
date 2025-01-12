namespace EasyStocks.Domain.Entities;

public class Admin : User
{
    private Admin() { }
    public Admin(FullName name, string email, string phoneNumber, Gender gender, Address address, NIN nin)
    : base(name, email, phoneNumber, gender, address, nin)
    {
    }

    public static Admin Create(FullName name, string email, string phoneNumber, Gender gender, Address address, NIN nin)
    {
        return new Admin(name, email, phoneNumber, gender, address, nin);
    }   
}