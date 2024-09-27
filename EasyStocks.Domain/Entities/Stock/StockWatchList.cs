namespace EasyStocks.Domain.Entities;

public class StockWatchList
{
    public int WatchlistId { get; set; }
    public int UserId { get; set; }
    public int StockId { get; set; }
    public EasyStockUser User { get; set; }
    public Stock Stock { get; set; }

    private StockWatchList() {}
    public StockWatchList(/*int watchlistId,*/ int userId, int stockId/*, EasyStockUser user, Stocks stock*/) 
    {
        //WatchlistId = watchlistId;
        UserId = userId;
        StockId = stockId;
        //User = user;
        //Stock = stock;
    }

    public static StockWatchList Create(/*int watchlistId,*/ int userId, int stockId/*, EasyStockUser user, Stocks stock*/)
    {
        return new StockWatchList(/*watchlistId,*/ userId, stockId/*, user, stock*/);
    }
}