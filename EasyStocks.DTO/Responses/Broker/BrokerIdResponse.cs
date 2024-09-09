namespace EasyStocks.DTO.Responses;

public class BrokerIdResponse : IServiceResponse
{
    public int BrokerId { get; set; } = 0;
    public string Email { get; set; }
    public string Token { get; set; }
}