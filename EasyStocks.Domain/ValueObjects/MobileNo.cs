namespace EasyStocks.Domain.ValueObjects;

public record MobileNo
{
    public string Value { get; }
    public int Hash { get; }

    private MobileNo()
    {
        Value = "";
        Hash = 0;
    }
    private MobileNo(string value)
    {
        Value = value;
        Hash = value.Trim().GetHashCode();
    }
    public static MobileNo Default() => new MobileNo();
    public static MobileNo Create(string value) => new MobileNo(value);
}