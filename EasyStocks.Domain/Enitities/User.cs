namespace EasyStocks.Domain.Enitities;

public class User
{
    public int UserId { get; private set; }
    public FullName Name { get; private set; } = FullName.Default();
    public Email Email { get; private set; } = Email.Default();
    public MobileNo MobileNumber { get; private set; } = MobileNo.Default();
    public Gender Gender{ get; private set; }
    public DateOnly DateOfBirth { get; private set; }

    internal User() { }
    internal User(FullName name, Email email, MobileNo mobilenumber, Gender gender, DateOnly dateOfBirth)
    {
        Name = name;
        Email = email;
        MobileNumber = mobilenumber;
        Gender = gender;
        DateOfBirth = dateOfBirth;
    }

    public static User Create(FullName name, Email email, MobileNo mobileNumber, Gender gender, DateOnly dateOfBirth)
    {
        return new User(name, email, mobileNumber, gender, dateOfBirth);
    }
}