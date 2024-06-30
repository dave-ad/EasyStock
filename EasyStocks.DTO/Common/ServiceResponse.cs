namespace EasyStocks.DTO.Common;

public class ServiceResponse<T> where T : IServiceResponse
{
    public bool IsSuccessful { get; set; } = default!;
    public string Error { get; set; } = default!;
    public string TechMessage { get; set; } = default!;
    public T Value { get; set; } = default!;
}

public class ServiceResponse
{
    public bool IsSuccessful { get; set; } = default!;
    public string Error { get; set; } = default!;
    public string TechMessage { get; set; } = default!;
}