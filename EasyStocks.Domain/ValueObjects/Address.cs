namespace EasyStocks.Domain.ValueObjects;

public record Address
{
    public string StreetNo { get; }
    public string StreetName { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    private Address() 
    {
        StreetNo = "";
        StreetName = "";
        City = "";
        State = "";
        ZipCode = "";
    }

    private Address(string streetNo, string streetname, string city, string state, string zipCode)
    {
        StreetNo = streetNo;
        StreetName = streetname;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Default() => new();
    public static Address Create(string streetNo, string streetName, string city, string state, string zipCode)
        => new(streetNo, streetName, city, state, zipCode);
}