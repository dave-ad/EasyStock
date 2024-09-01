namespace EasyStocks.Domain.Entities;

public class User : IdentityUser<int>
{
    public FullName Name { get; private set; } = FullName.Default();
    //public MobileNo MobileNumber { get; private set; } = MobileNo.Default();
    public Gender Gender { get; private set; }

    // User Specific Properties
    protected User() { }
    public User(FullName name, string email, string phoneNumber, Gender gender)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
    }

    //// Methods for creating and updating a easyStock user
    //public static User Create(FullName name, string email, MobileNo mobileNumber, Gender gender)
    //{
    //    return new User(name, email, mobileNumber, gender);
    //}

    public void Update(FullName name, string email, string phoneNumber, Gender gender)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
    }
}