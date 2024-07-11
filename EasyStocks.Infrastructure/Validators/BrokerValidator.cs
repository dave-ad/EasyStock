namespace EasyStocks.Infrastructure.Validator;

public class BrokerValidator
{
    public ServiceResponse<BrokerIdResponse> ValidateCorporate(CreateCorporateBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            resp.Error = "Company Name is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.CompanyEmail))
        {
            resp.Error = "Company Email is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidEmail(request.CompanyEmail))
        {
            resp.Error = "Invalid Email format. 'example@mail.com'";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.CompanyMobileNumber))
        {
            resp.Error = "Company Mobile Number is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidPhoneNumber(request.CompanyMobileNumber) || request.CompanyMobileNumber.Length != 11)
        {
            resp.Error = "Invalid Mobile Number format.";
            resp.IsSuccessful = false;
            return resp;
        }

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

        if (string.IsNullOrWhiteSpace(request.CACRegistrationNumber))
        {
            resp.Error = "CAC Registration is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidCACRegNo(request.CACRegistrationNumber))
        {
            resp.Error = "License number must start with two letters followed by five digits";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.StockBrokerLicense))
        {
            resp.Error = "Stock Broker License is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidStockBrokerLicense(request.StockBrokerLicense))
        {
            resp.Error = "Stock Broker License Number number must start with two letters followed by seven digits";
            resp.IsSuccessful = false;
            return resp;
        }

        foreach (var user in request.Users)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                resp.Error = "First Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                resp.Error = "Last Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                resp.Error = "Email is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidEmail(user.Email))
            {
                resp.Error = "Invalid Email format. 'example@mail.com'";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.MobileNumber))
            {
                resp.Error = "Mobile Number is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidPhoneNumber(user.MobileNumber) || user.MobileNumber.Length > 11)
            {
                resp.Error = "Invalid Mobile Number format.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.PositionInOrg))
            {
                resp.Error = "Position Held in Organization is required";
                resp.IsSuccessful = false;
                return resp;
            }
        }
        return new ServiceResponse<BrokerIdResponse> { IsSuccessful = true };
    }

    public ServiceResponse<BrokerIdResponse> ValidateIndividual(CreateIndividualBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        foreach (var user in request.Users)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                resp.Error = "First Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                resp.Error = "Last Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                resp.Error = "Email is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidEmail(user.Email))
            {
                resp.Error = "Invalid Email format. 'example@mail.com'";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.MobileNumber))
            {
                resp.Error = "Mobile Number is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidPhoneNumber(user.MobileNumber) || user.MobileNumber.Length > 11)
            {
                resp.Error = "Invalid Mobile Number format.";
                resp.IsSuccessful = false;
                return resp;
            }
        }

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

        if (string.IsNullOrWhiteSpace(request.StockBrokerLicenseNumber))
        {
            resp.Error = "Stock Broker License is required";
            resp.IsSuccessful = false;
            return resp;
        }

        if (!IsValidStockBrokerLicense(request.StockBrokerLicenseNumber))
        {
            resp.Error = "Stock Broker License Number number must start with two letters followed by seven digits";
            resp.IsSuccessful = false;
            return resp;
        }

        if (string.IsNullOrWhiteSpace(request.ProfessionalQualification))
        {
            resp.Error = "Your professional qualification is required.";
            resp.IsSuccessful = false;
            return resp;
        }

        return new ServiceResponse<BrokerIdResponse> { IsSuccessful = true };
    }

    public ServiceResponse<BrokerIdResponse> ValidateFreelance(CreateFreelanceBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        foreach (var user in request.Users)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                resp.Error = "First Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                resp.Error = "Last Name is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                resp.Error = "Email is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidEmail(user.Email))
            {
                resp.Error = "Invalid Email format. 'example@mail.com'";
                resp.IsSuccessful = false;
                return resp;
            }

            if (string.IsNullOrWhiteSpace(user.MobileNumber))
            {
                resp.Error = "Mobile Number is required.";
                resp.IsSuccessful = false;
                return resp;
            }

            if (!IsValidPhoneNumber(user.MobileNumber) || user.MobileNumber.Length > 11)
            {
                resp.Error = "Invalid Mobile Number format.";
                resp.IsSuccessful = false;
                return resp;
            }
        }

        if (string.IsNullOrWhiteSpace(request.ProfessionalQualification))
        {
            resp.Error = "Your professional qualification is required.";
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