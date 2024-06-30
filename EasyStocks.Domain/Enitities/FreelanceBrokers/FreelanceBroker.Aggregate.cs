namespace EasyStocks.Domain.Enitities;

public partial class FreelanceBroker : User, IAggregateRoot
{
    private FreelanceBroker() { }

    private FreelanceBroker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                            DateOnly dateOfBirth, string professionalQualification)
                            : base(name, email, mobileNumber, gender, dateOfBirth)
    {
        ProfessionalQualification = professionalQualification;
    }

    public static FreelanceBroker Create(FullName name, Email email, MobileNo mobileNumber,
                                            Gender gender, DateOnly dateOfBirth,
                                            string professionalQualification)
    {
        return new FreelanceBroker(name, email, mobileNumber, gender, dateOfBirth,
                                    professionalQualification);
    }
}