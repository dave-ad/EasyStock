
namespace EasyStocks.Domain.Entities;

public class Stock
{
    // Unique identifier for the stock
    public int StockId { get; private set; }
    public string? TickerSymbol { get; private set; }
    public string? CompanyName { get; private set; }
    public string? Exchange { get; private set; }

    // Prices
    public decimal OpeningPrice { get; private set; }
    public decimal ClosingPrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public decimal DayHigh { get; private set; }
    public decimal DayLow { get; private set; }
    public decimal YearHigh { get; private set; }
    public decimal YearLow { get; private set; }

    // Finacials
    public int OutstandingShares { get; private set; }
    public decimal MarketCapitalization => OutstandingShares * CurrentPrice; // Market cap is calculated from shares and price
    public decimal DividendYield { get; private set; } // Percentage return in dividends relative to the stock price
    public decimal EarningsPerShare { get; private set; }
    public decimal PriceEarningsRatio => CurrentPrice / EarningsPerShare; // P/E ratio based on earnings per share

    // Trading info
    public int Volume { get; private set; }
    public decimal Beta { get; private set; }


    //public List<StockWatchList> Watchlists { get; private set; } = new List<StockWatchList>();
    //public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

    internal Stock() {}
    internal Stock(string tickerSymbol, string companyName,
                    string exchange, decimal openingPrice, decimal closingPrice, 
                    decimal currentPrice, decimal dayhigh, decimal dayLow, 
                    decimal yearHigh, decimal yearLow,
                    int outstandingShares, 
                    decimal dividendYield, decimal earningsPerShare,
                    int volume, decimal beta)
    {
        TickerSymbol = tickerSymbol;
        CompanyName = companyName;
        Exchange = exchange;
        OpeningPrice = openingPrice;
        ClosingPrice = closingPrice;
        CurrentPrice = currentPrice;
        DayHigh = dayhigh;
        DayLow = dayLow;
        YearHigh = yearHigh;
        YearLow = yearLow;
        OutstandingShares = outstandingShares;
        DividendYield = dividendYield;
        EarningsPerShare = earningsPerShare;
        Volume = volume;
        Beta = beta;
    }

    public static Stock Create(string tickerSymbol, string companyName,
                               string exchange, decimal openingPrice,
                               decimal closingPrice, decimal currentPrice,
                               decimal dayHigh, decimal dayLow, decimal yearHigh,
                               decimal yearLow, int outstandingShares,
                               decimal dividendYield, decimal earningsPerShare,
                               int volume, decimal beta)
    {
        return new Stock(tickerSymbol, companyName, exchange, openingPrice,
                         closingPrice, currentPrice, dayHigh, dayLow, yearHigh,
                         yearLow, outstandingShares, dividendYield,
                         earningsPerShare, volume, beta);
    }

    public void Update(string tickerSymbol, string companyName, string exchange,
                       decimal openingPrice, decimal closingPrice, decimal currentPrice,
                       decimal dayHigh, decimal dayLow, decimal yearHigh,
                       decimal yearLow, int outstandingShares, decimal dividendYield,
                       decimal earningsPerShare, int volume, decimal beta)
    {
        TickerSymbol = tickerSymbol;
        CompanyName = companyName;
        Exchange = exchange;
        OpeningPrice = openingPrice;
        ClosingPrice = closingPrice;
        CurrentPrice = currentPrice;
        DayHigh = dayHigh;
        DayLow = dayLow;
        YearHigh = yearHigh;
        YearLow = yearLow;
        OutstandingShares = outstandingShares;
        DividendYield = dividendYield;
        EarningsPerShare = earningsPerShare;
        Volume = volume;
        Beta = beta;
    }
}