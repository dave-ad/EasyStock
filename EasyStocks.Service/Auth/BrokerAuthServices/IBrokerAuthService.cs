namespace EasyStocks.Service.BrokerAuthServices;

public interface IBrokerAuthService
{
    Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request);
    Task<ServiceResponse<BrokerIdResponse>> CreateIndividualBroker(CreateIndividualBrokerRequest request);

    Task<ServiceResponse<BrokerIdResponse>> CreateFreelanceBroker(CreateFreelanceBrokerRequest request);

    Task<LoginResponse> LoginCorporateBrokerAsync(BrokerLoginRequest request);
    Task<LoginResponse> LoginIndividualBrokerAsync(BrokerLoginRequest request);
    Task<LoginResponse> LoginFreelanceBrokerAsync(BrokerLoginRequest request);
}