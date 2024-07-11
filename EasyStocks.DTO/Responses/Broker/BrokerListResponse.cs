namespace EasyStocks.DTO.Responses;

public class BrokerListResponse : IServiceResponse
{
    public List<BrokerResponse> Brokers { get; set; } = new List<BrokerResponse>();
}