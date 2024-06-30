namespace EasyStocks.Domain.ValueObjects;

public record FullName
{
    public string Last { get; }
    public string First { get; }
    public string Others { get; }

    private FullName(string lastName, string firstName, string Othernames)
    {
        Last = lastName;
        First = firstName;
        Others = Othernames;
    }

    private FullName()
    {

        Last = "";
        First = "";
        Others = "";
    }

    public static FullName Default() => new();

    public static FullName Create(string lastName, string firstName, string Othernames)
        => new(lastName, firstName, Othernames);
}
