namespace EasyStocks.Domain.Entities;

public partial class Broker : IAggregateRoot
{
    private Broker() { }
    // Corporate Broker
    private Broker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                    string companyName, Email companyEmail, MobileNo companyMobileNumber,
                    Address companyAddress, CAC cacRegistrationNumber,
                    StockBrokerLicense stockBrokerLicenseNumber, DateOnly dateCertified,
                    string positionInOrg, DateOnly dateOfEmployment, AccountStatus status,
                    BrokerType brokerType)
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
        BrokerType = brokerType;
    }

    // Individual Broker
    public Broker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                    StockBrokerLicense stockBrokerLicenseNumber, DateOnly dateCertified,
                    AccountStatus status, Address businessAddress, string professionalQualification,
                    BrokerType brokerType)
    {
        Name = name;
        Email = email;
        MobileNumber = mobileNumber;
        Gender = gender;
        StockBrokerLicenseNumber = stockBrokerLicenseNumber;
        DateCertified = dateCertified;
        Status = status;
        BusinessAddress = businessAddress;
        ProfessionalQualification = professionalQualification;
        BrokerType = brokerType;
    }
    
    // Freelance Broker
    public Broker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                    AccountStatus status, string professionalQualification,
                    BrokerType brokerType)
    {
        Name = name;
        Email = email;
        MobileNumber = mobileNumber;
        Gender = gender;
        Status = status;
        ProfessionalQualification = professionalQualification;
        BrokerType = brokerType;
    }

    public static Broker CreateCorporate(FullName name, Email email, MobileNo mobileNumber,
                                            Gender gender, string companyName, Email companyEmail,
                                            MobileNo companyMobileNumber, Address companyAddress,
                                            CAC cacRegistrationNumber,
                                            StockBrokerLicense stockBrokerLicenseNumber,
                                            DateOnly dateCertified, string positionInOrg,
                                            DateOnly dateOfEmployment)
    {

        return new Broker(name, email, mobileNumber, gender, companyName,
                            companyEmail, companyMobileNumber, companyAddress,
                            cacRegistrationNumber, stockBrokerLicenseNumber, dateCertified,
                            positionInOrg, dateOfEmployment, AccountStatus.Pending, BrokerType.Corporate);
    }
    
    public static Broker CreateIndividual(FullName name, Email email, MobileNo mobileNumber,
                                            Gender gender, StockBrokerLicense stockBrokerLicenseNumber,
                                            DateOnly dateCertified, Address businessAddress, 
                                            string professionalQualification)
    {

        return new Broker(name, email, mobileNumber, gender,
                            stockBrokerLicenseNumber, dateCertified,
                            AccountStatus.Pending, businessAddress, professionalQualification, BrokerType.Individual);
    }
    
    public static Broker CreateFreelance(FullName name, Email email, MobileNo mobileNumber,
                                            Gender gender, string professionalQualification)
    {

        return new Broker(name, email, mobileNumber, gender,
                            AccountStatus.Pending, professionalQualification, BrokerType.Freelance);
    }
}