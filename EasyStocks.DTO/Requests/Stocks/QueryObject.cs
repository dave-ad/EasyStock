namespace EasyStocks.DTO.Requests;

public class QueryObject
{
    public string? TickerSymbol { get; set; }
    public string? CompanyName { get; set; }
    //public string StockType { get; set; }
    //public string TotalUnits { get; set; }
    //public decimal PricePerUnit { get; set; }
    //public DateTime OpeningDate { get; set; }
    //public DateTime ClosingDate { get; set; }
    //public string MinimumPurchase { get; set; }
    //public decimal InitialDeposit { get; set; }
    //public DateTime DateListed { get; set; }
    //public string ListedBy { get; set; }

    public string? SortBy { get; set; } // Add this property
    public bool IsDescending { get; set; } // Add this property
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
