namespace EasyStocks.Infrastructure.Validators;

public class StockValidator
{
    public ServiceResponse<StockResponse> ValidateStock(CreateStockRequest request)
    {
        var resp = new ServiceResponse<StockResponse>();

        if (string.IsNullOrWhiteSpace(request.TickerSymbol))
        {
            resp.Error = "Ticker Symbol is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            resp.Error = "Company Name is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.Exchange))
        {
            resp.Error = "Exchange platform is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.OpeningPrice <= 0)
        {
            resp.Error = "Opening Price must be greater than zero.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.ClosingPrice <= 0)
        {
            resp.Error = "Opening Price must be greater than zero.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.CurrentPrice <= 0)
        {
            resp.Error = "Current Price must be a positive value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.DayHigh < 0)
        {
            resp.Error = "Day High must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.DayLow < 0)
        {
            resp.Error = "Day Low must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.YearHigh < 0)
        {
            resp.Error = "Year High must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.YearLow < 0)
        {
            resp.Error = "Year Low must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.OutstandingShares <= 0)
        {
            resp.Error = "Outstanding Shares must be greater than zero.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.DividendYield < 0)
        {
            resp.Error = "Dividend Yield must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.EarningsPerShare < 0)
        {
            resp.Error = "Earnings Per Share must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.Volume < 0)
        {
            resp.Error = "Volume must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.Beta < 0)
        {
            resp.Error = "Beta must be a non-negative value.";
            resp.IsSuccessful = false;
            return resp;
        }

        return new ServiceResponse<StockResponse> { IsSuccessful = true };
    }   
}