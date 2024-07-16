namespace EasyStocks.Domain.ValueObjects;

public record CAC
{
    public string Value { get; }
    public int Hash{ get; }

    private CAC()
    {
        Value = "";
        Hash = 0;
    }
    private CAC(string value)
    {
        Value = value;
        Hash = Value.Trim().GetHashCode();
    }
    public static CAC Default() => new();
    public static CAC Create(string value) => new(value);
    public static CAC Update(string value) => new(value);
}