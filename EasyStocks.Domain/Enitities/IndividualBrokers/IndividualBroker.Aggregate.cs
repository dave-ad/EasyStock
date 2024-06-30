namespace EasyStocks.Domain.Enitities;

public partial class IndividualBroker : User, IAggregateRoot
{
    private IndividualBroker() { }

    private IndividualBroker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                                DateOnly dateOfBirth, Address businessAddress,
                                StockBrokerLicense stockBrokerLicenseNumber,
                                DateTime dateCertified, String professionalQualification) 
                                : base(name, email, mobileNumber, gender, dateOfBirth)
    {
        BusinessAddress = businessAddress;
        StockBrokerLicenseNumber = stockBrokerLicenseNumber;
        DateCertified = dateCertified;
        ProfessionalQualification = professionalQualification;
    }

    public static IndividualBroker Create(FullName name, Email email, MobileNo mobileNumber,
                                            Gender gender, DateOnly dateOfBirth,
                                            Address businessAddress,
                                            StockBrokerLicense stockBrokerLicenseNumber,
                                            DateTime dateCertified, string professionalQualification)
    {
        return new IndividualBroker(name, email, mobileNumber, gender, dateOfBirth,
                                    businessAddress, stockBrokerLicenseNumber, dateCertified, 
                                    professionalQualification);
    }
}