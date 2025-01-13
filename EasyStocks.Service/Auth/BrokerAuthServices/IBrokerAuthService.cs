namespace EasyStocks.Service.BrokerAuthServices;

public interface IBrokerAuthService
{
    Task<ServiceResponse<RegisterResponse>> CreateBrokerAsync(CreateBrokerRequest request);
    Task<ServiceResponse<LoginResponse>> LoginBrokerAsync(BrokerLoginRequest request);
    Task<ServiceResponse<LogoutResponse>> LogoutBrokerAsync(LogoutRequest request);
}