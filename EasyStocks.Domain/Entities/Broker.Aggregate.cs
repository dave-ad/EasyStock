namespace EasyStocks.Domain.Entities;

public partial class Broker : IAggregateRoot
{
    private Broker() { }
    //Corporate Broker
    private Broker(string companyName, Email companyEmail, MobileNo companyMobileNumber,
                    Address companyAddress, CAC cacRegistrationNumber,
                    StockBrokerLicense stockBrokerLicense, DateOnly dateCertified, AccountStatus status, BrokerType brokerType, List<User> users)
    {
        CompanyName = companyName;
        CompanyEmail = companyEmail;
        CompanyMobileNumber = companyMobileNumber;
        CompanyAddress = companyAddress;
        CACRegistrationNumber = cacRegistrationNumber;
        StockBrokerLicense = stockBrokerLicense;
        DateCertified = dateCertified;
        Status = status;
        BrokerType = brokerType;
        Users = users;
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
        StockBrokerLicense = stockBrokerLicenseNumber;
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

    public static Broker CreateCorporate(string companyName, Email companyEmail, MobileNo companyMobileNumber,
                                            Address companyAddress, CAC cacRegistrationNumber,
                                            StockBrokerLicense stockBrokerLicense, DateOnly dateCertified,
                                             List<User> users)
    {

        return new Broker(companyName, companyEmail, companyMobileNumber, companyAddress,
                            cacRegistrationNumber, stockBrokerLicense, dateCertified, 
                            AccountStatus.Pending, BrokerType.Corporate, users);
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