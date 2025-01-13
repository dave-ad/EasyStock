namespace EasyStocks.Service.BrokerServices;

public interface IBrokerService
{

    Task<ServiceResponse<BrokerListResponse>> GetAllBrokers();
    Task<ServiceResponse<BrokerResponse>> GetBrokerById(int brokerId);
    //Task<ServiceResponse<BrokerListResponse>> GetBrokersByType(BrokerRole? brokerType = null);
    Task<ServiceResponse> ChangeBrokerStatus(int brokerId, AccountStatus newStatus);
}