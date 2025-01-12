namespace EasyStocks.Domain.ValueObjects;

public record BrokerLicense
{
    public string Value { get; }
    public int Hash { get; }

    private BrokerLicense()
    {
        Value = "";
        Hash = 0;
    }
    private BrokerLicense(string value)
    {
        Value = value;
        Hash = Value.Trim().GetHashCode();
    }
    public static BrokerLicense Default() => new();
    public static BrokerLicense Create(string value) => new(value);
    public static BrokerLicense Update(string value) => new(value);
}
