namespace EasyStocks.Domain.Entities;

public class Stocks
{
    public int Id { get; private set; }
    public string StockTitle { get; private set; }
    public string CompanyName { get; private set; }
    public string StockType { get; private set; }
    public string TotalUnits { get; private set; }
    public decimal PricePerUnit { get; private set; }
    public DateTime OpeningDate { get; private set; }
    public DateTime ClosingDate { get; private set; }
    public string MinimumPurchase { get; private set; }
    public decimal InitialDeposit { get; private set; }
    public DateTime DateListed { get; private set; }
    public string ListedBy { get; private set; }

    internal Stocks() { }
    internal Stocks(string stockTitle, string companyName,
                    string stockType, string totalUnits,
                    decimal pricePerUnit, DateTime openingDate,
                    DateTime closingDate, string minimumPurchase, 
                    DateTime dateListed, string listedBy) 
    {
        StockTitle = stockTitle;
        CompanyName = companyName;
        StockType = stockType;
        TotalUnits = totalUnits;
        PricePerUnit = pricePerUnit;
        OpeningDate = openingDate;
        ClosingDate = closingDate;
        MinimumPurchase = minimumPurchase;
        InitialDeposit = CalculateInitialDeposit(pricePerUnit, minimumPurchase);
        DateListed = dateListed;
        ListedBy = listedBy;
    }

    private decimal CalculateInitialDeposit(decimal pricePerUnit, string minimumPurchase)
    {
        if (decimal.TryParse(minimumPurchase, out var minPurchaseUnits))
        {
            return pricePerUnit * minPurchaseUnits;
        }
        throw new ArgumentException("MinimumPurchase must be a valid number.");
    }

    public static Stocks Create(string stockTitle, string companyName,
                                string stockType, string totalUnits, 
                                decimal pricePerUnit, DateTime openingDate,
                                DateTime closingDate, string minimumPurchase, 
                                DateTime dateListed, string listedBy)
    {
        return new Stocks(stockTitle, companyName, 
                            stockType, totalUnits,
                            pricePerUnit, openingDate, 
                            closingDate, minimumPurchase, 
                            dateListed, listedBy);
    }
}