namespace EasyStocks.Domain.Entities;

public class Admin : User
{
    //public int SuperAdminLevel { get; private set; }
    //public List<string>? Permissions { get; private set; } = new List<string>();

    private Admin() { }
    public Admin(FullName name, string email, string phoneNumber, Gender gender/*, int superAdminLevel, List<string>? permissions*/)
    : base(name, email, phoneNumber, gender)
    {
        //SuperAdminLevel = superAdminLevel;
        //Permissions = permissions ?? new List<string>();
    }

    public static Admin Create(FullName name, string email, string phoneNumber, Gender gender/*, int superAdminLevel, List<string>? permissions*/)
    {
        return new Admin(name, email, phoneNumber, gender/*, superAdminLevel, permissions*/);
    }   
}