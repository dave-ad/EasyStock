namespace EasyStocks.Domain.ValueObjects;

public record StockBrokerLicense
{
    public string Value { get; }
    public int Hash { get; }

    private StockBrokerLicense()
    {
        Value = "";
        Hash = 0;
    }
    private StockBrokerLicense(string value)
    {
        Value = value;
        Hash = Value.Trim().GetHashCode();
    }
    public static StockBrokerLicense Default() => new();
    public static StockBrokerLicense Create(string value) => new(value);
}
