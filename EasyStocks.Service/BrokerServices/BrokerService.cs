using Microsoft.EntityFrameworkCore;

namespace EasyStocks.Service.BrokerServices;

public sealed class BrokerService : IBrokerService
{
    //private readonly IEasyStockAppDbContext _easyStockAppDbContext = easyStockAppDbContext;
    private readonly IEasyStockAppDbContext _easyStockAppDbContext;
    private readonly BrokerValidator _validator;

    public BrokerService(IEasyStockAppDbContext easyStockAppDbContext, BrokerValidator validator)
    {
        _easyStockAppDbContext = easyStockAppDbContext ?? throw new ArgumentNullException(nameof(easyStockAppDbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ServiceResponse<BrokerIdResponse>> CreateCorporateBroker(CreateCorporateBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateCorporate(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingCorporateBroker = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.CompanyEmail.Value.Trim().ToUpper() == request.CompanyEmail.Trim().ToUpper());

                if (existingCorporateBroker)
                {
                    return CreateDuplicateErrorResponse(resp, "Corporate broker");
                }

                var existingStaff = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.Email.Value.Trim().ToUpper() == request.Email.Trim().ToUpper());

                if (existingStaff)
                {
                    return CreateDuplicateErrorResponse(resp, "Staff");
                }

                var corporateBroker = CreateCorporateBrokerEntity(request);

                var retCorporateBroker = _easyStockAppDbContext.Brokers.Add(corporateBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retCorporateBroker == null || retCorporateBroker.Entity.BrokerId < 1)
                {
                    return CreateDatabaseErrorResponse(resp);
                }

                resp.Value = new BrokerIdResponse { BrokerId = retCorporateBroker.Entity.BrokerId };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
        }
        return resp;
    }
    
    public async Task<ServiceResponse<BrokerIdResponse>> CreateIndividualBroker(CreateIndividualBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateIndividual(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var existingBroker = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.Email.Value.Trim().ToUpper() == request.Email.Trim().ToUpper());

                if (existingBroker)
                {
                    return CreateDuplicateErrorResponse(resp, "broker");
                }

                var individualBroker = CreateIndividualBrokerEntity(request);

                var retIndividualBroker = _easyStockAppDbContext.Brokers.Add(individualBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retIndividualBroker == null || retIndividualBroker.Entity.BrokerId < 1)
                {
                    return CreateDatabaseErrorResponse(resp);
                }

                resp.Value = new BrokerIdResponse { BrokerId = retIndividualBroker.Entity.BrokerId };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                resp.Error = "Error Occurred";
                resp.TechMessage = ex.GetBaseException().Message;
                resp.IsSuccessful = false;
                return resp;
            }
        }
        return resp;
    }

    public async Task<ServiceResponse<BrokerIdResponse>> CreateFreelanceBroker(CreateFreelanceBrokerRequest request)
    {
        var resp = new ServiceResponse<BrokerIdResponse>();

        var validationResponse = _validator.ValidateFreelance(request);
        if (!validationResponse.IsSuccessful) return validationResponse;

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            try
            {
                var existingBroker = await _easyStockAppDbContext.Brokers.AnyAsync(b => b.Email.Value.Trim().ToUpper() ==  request.Email.Trim().ToUpper());

                if (existingBroker)
                {
                    return CreateDuplicateErrorResponse(resp, "broker");
                }

                var freelanceBroker = CreateFreelanceBrokerEntity(request);

                var retFreelanceBroker = _easyStockAppDbContext.Brokers.Add(freelanceBroker);
                await _easyStockAppDbContext.SaveChangesAsync();

                if (retFreelanceBroker == null || retFreelanceBroker.Entity.BrokerId < 1)
                {
                    return CreateDatabaseErrorResponse(resp);
                }

                resp.Value = new BrokerIdResponse { BrokerId = retFreelanceBroker.Entity.BrokerId };
                resp.IsSuccessful = true;

                transaction.Complete();
            }
            catch (Exception ex)
            {
                return CreateExceptionResponse(resp, ex);
            }
        }
        return resp;
    }

    private static ServiceResponse<BrokerIdResponse> CreateDuplicateErrorResponse(ServiceResponse<BrokerIdResponse> resp, string entityType)
    {
        resp.Error = $"Duplicate Error. A {entityType} with the provided details already exists.";
        resp.TechMessage = $"Duplicate Error. A {entityType} with the provided details already exists.";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<BrokerIdResponse> CreateDatabaseErrorResponse(ServiceResponse<BrokerIdResponse> resp)
    {
        resp.Error = "Error Occurred";
        resp.TechMessage = "Unknown Database Error";
        resp.IsSuccessful = false;
        return resp;
    }

    private static ServiceResponse<BrokerIdResponse> CreateExceptionResponse(ServiceResponse<BrokerIdResponse> resp, Exception ex)
    {
        resp.Error = "Error Occurred";
        resp.TechMessage = ex.GetBaseException().Message;
        resp.IsSuccessful = false;
        return resp;
    }

    private Broker CreateCorporateBrokerEntity(CreateCorporateBrokerRequest request)
    {
        var companyEmail = Email.Create(request.CompanyEmail);
        var companyMobileNo = MobileNo.Create(request.CompanyMobileNumber);
        var companyAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var cac = CAC.Create(request.CACRegistrationNumber);
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicenseNumber);

        var fullName = FullName.Create(request.LastName, request.FirstName, request.OtherNames);
        var email = Email.Create(request.Email);
        var mobileNo = MobileNo.Create(request.MobileNumber);

        return Broker.CreateCorporate(fullName, email, mobileNo, request.Gender, request.CompanyName, companyEmail, companyMobileNo, companyAddress, cac, stockBrokerLicense, request.DateCertified, request.PositionInOrg, request.DateOfEmployment);
    }

    private Broker CreateIndividualBrokerEntity(CreateIndividualBrokerRequest request)
    {
        var fullName = FullName.Create(request.LastName, request.FirstName, request.OtherNames);
        var email = Email.Create(request.Email);
        var mobileNo = MobileNo.Create(request.MobileNumber);

        var businessAddress = Address.Create(request.StreetNo, request.StreetName, request.City, request.State, request.ZipCode);
        var stockBrokerLicense = StockBrokerLicense.Create(request.StockBrokerLicenseNumber);

        return Broker.CreateIndividual(fullName, email, mobileNo, request.Gender, stockBrokerLicense, request.DateCertified, businessAddress, request.ProfessionalQualification);
    }

    private Broker CreateFreelanceBrokerEntity(CreateFreelanceBrokerRequest request)
    {
        var fullName = FullName.Create(request.LastName, request.FirstName, request.OtherNames);
        var email = Email.Create(request.Email);
        var mobileNo = MobileNo.Create(request.MobileNumber);

        return Broker.CreateFreelance(fullName, email, mobileNo, request.Gender, request.ProfessionalQualification);
    }
}