namespace EasyStocks.Domain.ValueObjects;

public record Email
{
    public string Value { get; }
    public int Hash { get; }

    private Email()
    {
        Value = "";
        Hash = 0;
    }
    private Email(string value)
    {
        Value = value;
        Hash = Value.Trim().GetHashCode();
    }
    public static Email Default() => new();
    public static Email Create(string value) => new(value);
}