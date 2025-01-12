namespace EasyStocks.Infrastructure.Validators;

public class BrokerValidator
{
    public ServiceResponse<BrokerIdResponse> ValidateBroker(CreateBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        // Address Property
        if (string.IsNullOrWhiteSpace(request.StreetNo))
        {
            resp.Error = "Street Number is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.StreetName))
        {
            resp.Error = "Street Name is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.City))
        {
            resp.Error = "City is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.State))
        {
            resp.Error = "State is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.ZipCode))
        {
            resp.Error = "ZipCode is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (request.ZipCode.Length != 6 || !request.ZipCode.All(char.IsDigit))
        {
            resp.Error = "ZipCode must be 6 digits.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.BrokerLicense))
        {
            resp.Error = "Stock Broker License is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidStockBrokerLicense(request.BrokerLicense))
        {
            resp.Error = "Stock Broker License Number number must start with two letters followed by seven digits";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            resp.Error = "First Name is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            resp.Error = "Last Name is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            resp.Error = "Email is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidEmail(request.Email))
        {
            resp.Error = "Invalid Email format. 'example@mail.com'";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            resp.Error = "Mobile Number is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidPhoneNumber(request.PhoneNumber) || request.PhoneNumber.Length > 11)
        {
            resp.Error = "Invalid Mobile Number format.";
            resp.IsSuccessful = false;
            return resp;
        }
        
        return new ServiceResponse<BrokerIdResponse> { IsSuccessful = true };
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^(070|080|081|090|091)\d{8}$");
    }

    private bool IsValidCACRegNo(string cacRegistrationNumber)
    {
        return Regex.IsMatch(cacRegistrationNumber, @"^[A-Za-z]{2}\d{5}$");
    }

    private bool IsValidStockBrokerLicense(string stockBrokerLicense)
    {
        return Regex.IsMatch(stockBrokerLicense, @"^[A-Za-z]{2}\d{7}$");
    }
}