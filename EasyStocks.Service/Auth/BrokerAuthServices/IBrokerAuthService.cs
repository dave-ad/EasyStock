namespace EasyStocks.Service.BrokerAuthServices;

public interface IBrokerAuthService
{
    Task<ServiceResponse<RegisterResponse>> CreateBrokerAsync(CreateBrokerRequest request);

    Task<LoginResponse> LoginBrokerAsync(BrokerLoginRequest request, BrokerRole brokerRole);
    Task<LogoutResponse> LogoutBrokerAsync(LogoutRequest request);
}