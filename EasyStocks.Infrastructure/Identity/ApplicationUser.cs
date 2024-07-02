//namespace EasyStocks.Infrastructure.Identity;

//// Instead of creating a new class, want ApplicationUser class to inherit from
//// my domain User class. This way, I can keep all properties in one place and still
//// use Identity
//public class ApplicationUser : IdentityUser<int> // Assuming UserId is int
//{
//    public FullName Name { get; set; } = FullName.Default();
//    public MobileNo MobileNumber { get; set; } = MobileNo.Default();
//    public Gender Gender { get; set; }
//    //public DateOnly DateOfBirth { get; set; }

//    public ApplicationUser() { }

//    public ApplicationUser(string userName, string email, FullName name, MobileNo mobileNumber, Gender gender)
//        : base(userName)
//    {
//        Email = email;
//        Name = name;
//        MobileNumber = mobileNumber;
//        Gender = gender;
//    }
//}