namespace EasyStocks.Web.Controllers;

public class BrokerController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BrokerController> _logger;
    private readonly IBrokerService _brokerService;

    public BrokerController(SignInManager<User> signInManager,UserManager<User> userManager, ILogger<BrokerController> logger, IBrokerService brokerService)
    {
        _signInManager = signInManager;
        _brokerService = brokerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _brokerService.GetAllBrokers();

        if (response.IsSuccessful)
        {
            return View(response.Value);
        }
        else
        {
            _logger.LogError("Failed to retrieve brokers: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve brokers");
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> BrokerDetails(int id)
    {
        var response = await _brokerService.GetBrokerById(id);

        if (response.IsSuccessful)
        {
            return View(response.Value);
        }
        else
        {
            _logger.LogError("Failed to retrieve broker: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker");
            return View();
        }
    }

    [HttpGet("GetBrokersByType/{brokerType}")]
    public async Task<IActionResult> GetBrokersByType(BrokerType brokerType)
    {
        _logger.LogInformation("BrokerType: {BrokerType}", brokerType);

        var response = await _brokerService.GetBrokersByType(brokerType);
        if (response.IsSuccessful)
        {
            // Determine which view to render based on brokerType
            switch (brokerType)
            {
                case BrokerType.Corporate:
                    return View("GetCorporateBrokers", response.Value);
                case BrokerType.Individual:
                    return View("GetIndividualBrokers", response.Value);
                case BrokerType.Freelance:
                    return View("GetFreelanceBrokers", response.Value);
                default:
                    _logger.LogError("Invalid broker type: {BrokerType}", brokerType);
                    ModelState.AddModelError(string.Empty, "Invalid broker type");
                    return View();
            } // Pass BrokerListResponse directly to the corresponding view
        }
        else
        {
            _logger.LogError("Failed to retrieve broker: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker");
            return View(Index);
        }
    }

    public async Task<IActionResult> CorporateBrokers()
    {
        var response = await _brokerService.GetBrokersByType(BrokerType.Corporate);

        if (!response.IsSuccessful)
        {
            // Handle error, maybe return a NotFound view or redirect
            return NotFound();
        }

        return View(response.Value); // Assuming you have a view named CorporateBrokers.cshtml
    }

    public async Task<IActionResult> IndividualBrokers()
    {
        var response = await _brokerService.GetBrokersByType(BrokerType.Individual);

        if (!response.IsSuccessful)
        {
            // Handle error, maybe return a NotFound view or redirect
            return NotFound();
        }

        return View(response.Value); // Assuming you have a view named IndividualBrokers.cshtml
    }

    public async Task<IActionResult> FreelanceBrokers()
    {
        var response = await _brokerService.GetBrokersByType(BrokerType.Freelance);

        if (!response.IsSuccessful)
        {
            // Handle error, maybe return a NotFound view or redirect
            return NotFound();
        }

        return View(response.Value); // Assuming you have a view named FreelanceBrokers.cshtml
    }

    [HttpGet]
    public ActionResult CreateCorporateBroker()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCorporateBroker(CreateCorporateBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(CreateCorporateBroker), request);
        }

        try
        {
            ServiceResponse<BrokerIdResponse> resp = await _brokerService.CreateCorporateBroker(request);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker added successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to add broker: {Message}", resp.TechMessage);
                ModelState.AddModelError(string.Empty, "Failed to add broker");
                return View(nameof(CreateCorporateBroker), request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while adding new broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while adding new broker.");
            return View(nameof(CreateCorporateBroker), request);
        }
    }

    [HttpGet]
    public ActionResult CreateIndividualBroker()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateIndividualBroker(CreateIndividualBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(CreateIndividualBroker), request);
        }

        try
        {
            ServiceResponse<BrokerIdResponse> resp = await _brokerService.CreateIndividualBroker(request);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker added successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to add broker: {Message}", resp.TechMessage);
                ModelState.AddModelError(string.Empty, "Failed to add broker");
                return View(nameof(CreateIndividualBroker), request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while adding new broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while adding new broker.");
            return View(nameof(CreateIndividualBroker), request);
        }
    }

    [HttpGet]
    public ActionResult CreateFreelanceBroker()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFreelanceBroker(CreateFreelanceBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(CreateFreelanceBroker), request);
        }

        try
        {
            ServiceResponse<BrokerIdResponse> resp = await _brokerService.CreateFreelanceBroker(request);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker added successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to add broker: {Message}", resp.TechMessage);
                ModelState.AddModelError(string.Empty, "Failed to add broker");
                return View(nameof(CreateFreelanceBroker), request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while adding new broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while adding new broker.");
            return View(nameof(CreateFreelanceBroker), request);
        }
    }

    [HttpGet]
    [Route("Broker/GetBrokerById/{id}")]
    public async Task<IActionResult> GetBrokerById(int id)
    {
        var resp = await _brokerService.GetBrokerById(id);

        if (!resp.IsSuccessful)
        {
            _logger.LogError("Failed to fetch broker: {Message}", resp.Error);
            return NotFound(resp.Error);
        }

        return Ok(resp.Value);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCorporateBroker(int id)
    {
        var response = await _brokerService.GetBrokerById(id);

        if (response.IsSuccessful)
        {
            var updateRequest = new UpdateCorporateBrokerRequest
            {
                BrokerId = response.Value.BrokerId,
                CompanyName = response.Value.CompanyName,
                CompanyEmail = response.Value.CompanyEmail,
                CompanyMobileNumber = response.Value.CompanyMobileNumber,
                StreetNo = response.Value.CompanyAddress?.StreetNo,
                StreetName = response.Value.CompanyAddress?.StreetName,
                City = response.Value.CompanyAddress?.City,
                State = response.Value.CompanyAddress?.State,
                ZipCode = response.Value.CompanyAddress?.ZipCode,
                CACRegistrationNumber = response.Value.CACRegistrationNumber,
                StockBrokerLicense = response.Value.StockBrokerLicense,
                DateCertified = response.Value.DateCertified
            };

            return View(updateRequest);
        }
        else
        {
            _logger.LogError("Failed to retrieve broker for update: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker for update");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCorporateBroker(UpdateCorporateBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var response = await _brokerService.UpdateCorporateBroker(request);

            if (response.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to update broker: {Error}", response.Error);
                ModelState.AddModelError(string.Empty, "Failed to update broker");
                return View(request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while updating broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while updating broker.");
            return View(request);
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateIndividualBroker(int id)
    {
        var resp = await _brokerService.GetBrokerById(id);

        if (resp.IsSuccessful)
        {
            var updateRequest = new UpdateIndividualBrokerRequest
            {
                BrokerId = resp.Value.BrokerId,
                StreetNo = resp.Value.BusinessAddress?.StreetNo,
                StreetName = resp.Value.BusinessAddress?.StreetName,
                City = resp.Value.BusinessAddress?.City,
                State = resp.Value.BusinessAddress?.State,
                ZipCode = resp.Value.BusinessAddress?.ZipCode,
                StockBrokerLicense = resp.Value.StockBrokerLicense,
                DateCertified = resp.Value.DateCertified
            };
            _logger.LogInformation("Update Request: {@updateRequest}", updateRequest);
            return View(updateRequest);
        }
        else
        {
            _logger.LogError("Failed to retrieve broker for update: {Error}", resp.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker for update");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateIndividualBroker(UpdateIndividualBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var response = await _brokerService.UpdateIndividualBroker(request);

            if (response.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to update broker: {Error}", response.Error);
                ModelState.AddModelError(string.Empty, "Failed to update broker");
                return View(request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while updating broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while updating broker.");
            return View(request);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateFreelanceBroker(int id)
    {
        var resp = await _brokerService.GetBrokerById(id);

        if (resp.IsSuccessful)
        {
            var updateRequest = new UpdateFreelanceBrokerRequest
            {
                BrokerId = resp.Value.BrokerId,
                ProfessionalQualification = resp.Value.ProfessionalQualification
            };

            return View(updateRequest);
        }
        else
        {
            _logger.LogError("Failed to retrieve broker for update: {Error}", resp.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker for update");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateFreelanceBroker(UpdateFreelanceBrokerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var resp = await _brokerService.UpdateFreelanceBroker(request);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to update broker: {Error}", resp.Error);
                ModelState.AddModelError(string.Empty, "Failed to update broker");
                return View(request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while updating broker.");
            ModelState.AddModelError(string.Empty, "An error occurred while updating broker.");
            return View(request);
        }
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteBroker(int id)
    //{
    //    try
    //    {
    //        var response = await _brokerService.DeleteBroker(id);

    //        if (response.IsSuccessful)
    //        {
    //            TempData["SuccessMessage"] = "Broker deleted successfully.";
    //            return RedirectToAction(nameof(Index));
    //        }
    //        else
    //        {
    //            _logger.LogError("Failed to delete broker: {Error}", response.Error);
    //            TempData["ErrorMessage"] = "Failed to delete broker.";
    //            return RedirectToAction(nameof(Index));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An exception occurred while deleting broker.");
    //        TempData["ErrorMessage"] = "An error occurred while deleting broker.";
    //        return RedirectToAction(nameof(Index));
    //    }
    //}
}