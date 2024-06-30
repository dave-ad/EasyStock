namespace EasyStocks.Domain.Enitities;

public partial class CorporateBroker : User, IAggregateRoot
{
    private CorporateBroker() { }

    private CorporateBroker(FullName name, Email email, MobileNo mobileNumber, Gender gender,
                            DateOnly dateOfBirth, string companyName, Email companyEmail,
                            MobileNo companyMobileNumber, Address corporateAddress,
                            Address companyAddress, CAC cacRegistrationNumber,
                            string positioninOrg, DateOnly dateOfEmployment,
                            StockBrokerLicense stockBrokerLicenseNumber, DateTime dateCertified,
                            AccountStatus status) 
                            : base(name, email, mobileNumber, gender, dateOfBirth)
    {
        CompanyName = companyName;
        CompanyEmail = companyEmail;
        CompanyMobileNumber = companyMobileNumber;
        CorporateAddress = corporateAddress;
        CompanyAddress = companyAddress;
        CACRegistrationNumber = cacRegistrationNumber;
        PositionInOrg = positioninOrg;
        DateOfEmployment = dateOfEmployment;
        StockBrokerLicenseNumber = stockBrokerLicenseNumber;
        DateCertified = dateCertified;
        Status = status;
    }

    public static CorporateBroker Create(FullName name, Email email, MobileNo mobileNumber,
                                        Gender gender, DateOnly dateOfBirth, 
                                        string companyName, Email companyEmail, MobileNo companyMobileNumber,
                                        Address corporateAddress, Address companyAddress,
                                        CAC cacRegistrationNumber, string positionInOrg, 
                                        DateOnly dateOfEmployment,
                                        StockBrokerLicense stockBrokerLicenseNumber,
                                        DateTime dateCertified)
    {
        return new CorporateBroker(name, email, mobileNumber, gender, dateOfBirth, companyName,
                                    companyEmail, companyMobileNumber,corporateAddress, 
                                    companyAddress, cacRegistrationNumber, positionInOrg,
                                    dateOfEmployment, stockBrokerLicenseNumber, dateCertified, 
                                    AccountStatus.Pending);
    }
}