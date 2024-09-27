namespace EasyStocks.DTO.Requests;

public class CreateStockRequest
{
    public string TickerSymbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public decimal OpeningPrice { get; set; }
    public decimal ClosingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal DayHigh { get; set; }
    public decimal DayLow { get; set; }
    public decimal YearHigh { get; set; }
    public decimal YearLow { get; set; }
    public int OutstandingShares { get; set; }
    public decimal DividendYield { get; set; }
    public decimal EarningsPerShare { get; set; }
    public int Volume { get; set; }
    public decimal Beta { get; set; }
}