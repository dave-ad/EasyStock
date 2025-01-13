namespace EasyStocks.Domain.Entities;

public partial class Broker : IAggregateRoot
{
    private Broker() { }

    private Broker(FullName name, string email, string phoneNumber, 
                    Gender gender, Address address, NIN nin, 
                    BrokerLicense brokerLicense, DateOnly? dateCertified, 
                    string professionalQualification, AccountStatus status)
                    : base(name, email, phoneNumber, gender, address, nin)
    {
        BrokerLicense = brokerLicense;
        DateCertified = dateCertified;
        ProfessionalQualification = professionalQualification;
        Status = status;
    }

    public static Broker Create(FullName name, string email, string phoneNumber, 
                                Gender gender, Address address, NIN nin,
                                BrokerLicense brokerLicense, DateOnly? dateCertified,
                                string professionalQualification, AccountStatus status) 
    {

        return new Broker(name, email, phoneNumber, gender, 
                            address, nin, brokerLicense, dateCertified, 
                            professionalQualification, AccountStatus.Pending);
    }

    public void Update(FullName name, string email, string phoneNumber,
                        Address address, Gender gender, NIN nin, 
                        BrokerLicense brokerLicense, DateOnly? dateCertified, 
                        string professionalQualification)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Gender = gender;
        NIN = nin;
        BrokerLicense = brokerLicense;
        DateCertified = dateCertified;
        ProfessionalQualification = professionalQualification;
    }


    // Method to update status
    public void UpdateStatus(AccountStatus newStatus)
    {
        Status = newStatus;
    }
}