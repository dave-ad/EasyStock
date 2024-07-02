namespace EasyStocks.Domain.Entities;

public partial class Broker : IAggregateRoot
{
    private Broker() { }
    private Broker(FullName name, Email email, MobileNo mobileNumber, Gender gender, 
                    string companyName, Email companyEmail, MobileNo companyMobileNumber,
                    Address companyAddress, CAC cacRegistrationNumber,
                    StockBrokerLicense stockBrokerLicenseNumber, DateTime dateCertified,
                    string positionInOrg, DateOnly dateOfEmployment, AccountStatus status, 
                    Address businessAddress, string professionalQualification, BrokerType brokerType)
    {
        Name = name;
        Email = email;
        MobileNumber = mobileNumber;
        Gender = gender;
        CompanyName = companyName;
        CompanyEmail = companyEmail;
        CompanyMobileNumber = companyMobileNumber;
        CompanyAddress = companyAddress;
        CACRegistrationNumber = cacRegistrationNumber;
        StockBrokerLicenseNumber = stockBrokerLicenseNumber;
        DateCertified = dateCertified;
        PositionInOrg = positionInOrg;
        DateOfEmployment = dateOfEmployment;
        Status = status;
        BusinessAddress = businessAddress;
        ProfessionalQualification = professionalQualification;
        BrokerType = brokerType;
    }

    public static Broker Create(FullName name, Email email, MobileNo mobileNumber,
                                Gender gender,
                                string companyName, Email companyEmail,
                                MobileNo companyMobileNumber, Address companyAddress,
                                CAC cacRegistrationNumber,
                                StockBrokerLicense stockBrokerLicenseNumber,
                                DateTime dateCertified, string positionInOrg,
                                DateOnly dateOfEmployment, AccountStatus status,
                                Address businessAddress, string professionalQualification,
                                BrokerType brokerType)
    {
        return new Broker(name, email, mobileNumber, gender, companyName,
                            companyEmail, companyMobileNumber, companyAddress,
                            cacRegistrationNumber, stockBrokerLicenseNumber, dateCertified,
                            positionInOrg,
                            dateOfEmployment, status, businessAddress, professionalQualification, brokerType);
    }
}