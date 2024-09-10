namespace EasyStocks.Domain.Entities;

public partial class Broker : IAggregateRoot
{
    private Broker() { }
    //Corporate Broker
    private Broker(string companyName, Email companyEmail, MobileNo companyMobileNumber,
                    Address companyAddress, CAC cacRegistrationNumber,
                    StockBrokerLicense stockBrokerLicense, DateOnly dateCertified, AccountStatus status, BrokerRole brokerType, List<BrokerAdmin> users)
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
    private Broker(List<BrokerAdmin> users,
                    StockBrokerLicense stockBrokerLicenseNumber, DateOnly dateCertified,
                    AccountStatus status, Address businessAddress,
                    string professionalQualification, BrokerRole brokerType)
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
    private Broker(List<BrokerAdmin> users, AccountStatus status,
                    string professionalQualification, BrokerRole brokerType)
    {
        Users = users;
        Status = status;
        ProfessionalQualification = professionalQualification;
        BrokerType = brokerType;
    }

    public static Broker CreateCorporate(string companyName, Email companyEmail, MobileNo companyMobileNumber,
                                            Address companyAddress, CAC cacRegistrationNumber,
                                            StockBrokerLicense stockBrokerLicense, DateOnly dateCertified,
                                             List<BrokerAdmin> users)
    {

        return new Broker(companyName, companyEmail, companyMobileNumber, companyAddress,
                            cacRegistrationNumber, stockBrokerLicense, dateCertified,
                            AccountStatus.Pending, BrokerRole.CorporateBroker, users);
    }

    public static Broker CreateIndividual(List<BrokerAdmin> users,
                                            StockBrokerLicense stockBrokerLicenseNumber,
                                            DateOnly dateCertified, Address businessAddress,
                                            string professionalQualification)
    {

        return new Broker(users,
                            stockBrokerLicenseNumber, dateCertified, AccountStatus.Pending,
                            businessAddress, professionalQualification, BrokerRole.IndividualBroker);
    }

    public static Broker CreateFreelance(List<BrokerAdmin> users, string professionalQualification)
    {

        return new Broker(users,
                            AccountStatus.Pending, professionalQualification, BrokerRole.FreelanceBroker);
    }

    public void UpdateCorporate(string companyName, Email companyEmail, MobileNo companyMobileNumber,
                                    Address companyAddress, CAC cacRegistrationNumber,
                                    StockBrokerLicense stockBrokerLicense, DateOnly? dateCertified)
    {
        CompanyName = companyName;
        CompanyEmail = companyEmail;
        CompanyMobileNumber = companyMobileNumber;
        CompanyAddress = companyAddress;
        CACRegistrationNumber = cacRegistrationNumber;
        StockBrokerLicense = stockBrokerLicense;
        DateCertified = dateCertified;
    }

    public void UpdateIndividual(Address businessAddress, StockBrokerLicense stockBrokerLicense,
                                 DateOnly? dateCertified, string professionalQualification)
    {
        BusinessAddress = businessAddress;
        StockBrokerLicense = stockBrokerLicense;
        DateCertified = dateCertified;
        ProfessionalQualification = professionalQualification;
    }

    public void UpdateFreelance(string professionalQualification)
    {
        ProfessionalQualification = professionalQualification;
    }

    // Method to update status
    public void UpdateStatus(AccountStatus newStatus)
    {
        Status = newStatus;
    }
}