namespace EasyStocks.DTO.Responses;

public class StockResponse : IServiceResponse
{
    public int StockId { get; set; }
    public string? TickerSymbol { get; set; }
    public string? CompanyName { get; set; }
    public string? Exchange { get; set; }

    // Pricing Information
    public decimal OpeningPrice { get; set; }
    public decimal ClosingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal DayHigh { get; set; }
    public decimal DayLow { get; set; }
    public decimal YearHigh { get; set; }
    public decimal YearLow { get; set; }

    // Financial Details
    public int OutstandingShares { get; set; }
    public decimal MarketCapitalization { get; set; }
    public decimal DividendYield { get; set; }
    public decimal EarningsPerShare { get; set; }
    public decimal PriceEarningsRatio { get; set; }
    public int Volume { get; set; }
    public decimal Beta { get; set; }
}