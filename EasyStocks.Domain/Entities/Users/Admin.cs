namespace EasyStocks.Domain.Entities;

public class Admin : User
{
    public int SuperAdminLevel { get; private set; }
    public List<string>? Permissions { get; private set; } = new List<string>();

    private Admin() { }
    public Admin(FullName name, string email, MobileNo mobileNumber, Gender gender, int superAdminLevel, List<string>? permissions)
    : base(name, email, mobileNumber, gender)
    {
        SuperAdminLevel = superAdminLevel;
        Permissions = permissions ?? new List<string>();
    }

    public static Admin Create(FullName name, string email, MobileNo mobileNumber, Gender gender, int superAdminLevel, List<string>? permissions)
    {
        return new Admin(name, email, mobileNumber, gender, superAdminLevel, permissions);
    }   
}