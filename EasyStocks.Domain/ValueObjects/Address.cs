namespace EasyStocks.Domain.ValueObjects;

public record Address
{
    public string StreetNo { get; init; }
    public string StreetName { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }

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