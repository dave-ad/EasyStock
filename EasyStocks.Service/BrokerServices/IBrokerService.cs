namespace EasyStocks.Service.BrokerServices;

public interface IBrokerService
{
    Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request);
    Task<ServiceResponse<BrokerIdResponse>> CreateIndividualBroker(CreateIndividualBrokerRequest request);
    Task<ServiceResponse<BrokerIdResponse>> CreateFreelanceBroker(CreateFreelanceBrokerRequest request);
    Task<ServiceResponse<BrokerListResponse>> GetAllBrokers();
    Task<ServiceResponse<BrokerResponse>> GetBrokerById(int brokerId);
    Task<ServiceResponse<BrokerListResponse>> GetBrokersByType(BrokerRole? brokerType = null);
    Task<ServiceResponse<BrokerResponse>> UpdateCorporateBroker(UpdateCorporateBrokerRequest request);
    Task<ServiceResponse<BrokerResponse>> UpdateIndividualBroker(UpdateIndividualBrokerRequest request);
    Task<ServiceResponse<BrokerResponse>> UpdateFreelanceBroker(UpdateFreelanceBrokerRequest request);
    Task<ServiceResponse> ChangeBrokerStatus(int brokerId, AccountStatus newStatus);
}