namespace EasyStocks.Infrastructure.Validators;

public class StockValidator
{
    public ServiceResponse<StockIdResponse> ValidateStock(CreateStockRequest request)
    {
        var resp = new ServiceResponse<StockIdResponse>();

        if (string.IsNullOrWhiteSpace(request.StockTitle))
        {
            resp.Error = "Stock Title is required.";
            resp.IsSuccessful = false;
            return resp;
        }
        
        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            resp.Error = "Company Name is required.";
            resp.IsSuccessful = false;
            return resp;
        }
        
        if (string.IsNullOrWhiteSpace(request.StockType))
        {
            resp.Error = "Stock Type is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        var validStockTypes = new List<string> { "Common", "Preferred" };
        if (!validStockTypes.Contains(request.StockType))
        {
            resp.Error = "Invalid Stock Type. Stock type can only be 'Common' or 'Preferred'.";
            resp.IsSuccessful = false;
            return resp;
        }


        if (string.IsNullOrWhiteSpace(request.TotalUnits))
        {
            resp.Error = "Total Units is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.TotalUnits.Length <= 0 || !request.TotalUnits.All(char.IsDigit))
        {
            resp.Error = "Total Units must be greater than zero.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.PricePerUnit <= 0)
        {
            resp.Error = "Price Per Unit must be a positive value.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.OpeningDate == default)
        {
            resp.Error = "Opening Date is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.ClosingDate == default)
        {
            resp.Error = "Closing Date is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.OpeningDate >= request.ClosingDate)
        {
            resp.Error = "Opening Date must be before Closing Date.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.MinimumPurchase))
        {
            resp.Error = "Total Units is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!request.MinimumPurchase.All(char.IsDigit))
        {
            resp.Error = "Minimum purchase is 10 units.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.DateListed == default)
        {
            resp.Error = "Date Listed is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.ListedBy))
        {
            resp.Error = "Listed By is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        return new ServiceResponse<StockIdResponse> { IsSuccessful = true};
    }
}