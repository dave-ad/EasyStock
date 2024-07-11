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
    private Broker(List<User> users, 
                    StockBrokerLicense stockBrokerLicenseNumber, DateOnly dateCertified,
                    AccountStatus status, Address businessAddress,
                    string professionalQualification, BrokerType brokerType)
    {
        Users = users;
        StockBrokerLicense = stockBrokerLicenseNumber;
        DateCertified = dateCertified;
        Status = status;
        BusinessAddress = businessAddress;
        ProfessionalQualification = professionalQualification;
        BrokerType = brokerType;
    }
    
    // Freelance Broker
    private Broker(List<User> users, AccountStatus status,
                    string professionalQualification, BrokerType brokerType)
    {
        Users = users;
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
    
    public static Broker CreateIndividual(List<User> users,
                                            StockBrokerLicense stockBrokerLicenseNumber,
                                            DateOnly dateCertified, Address businessAddress, 
                                            string professionalQualification)
    {

        return new Broker(users,
                            stockBrokerLicenseNumber, dateCertified, AccountStatus.Pending,
                            businessAddress, professionalQualification, BrokerType.Individual);
    }
    
    public static Broker CreateFreelance(List<User> users, string professionalQualification)
    {

        return new Broker(users,
                            AccountStatus.Pending, professionalQualification, BrokerType.Freelance);
    }
}