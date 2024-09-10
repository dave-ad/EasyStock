namespace EasyStocks.Web.Controllers;

public class BrokerController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BrokerController> _logger;
    private readonly IBrokerService _brokerService;

    public BrokerController(UserManager<User> userManager, ILogger<BrokerController> logger, IBrokerService brokerService)
    {
        _userManager = userManager;
        _brokerService = brokerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var resp = await _brokerService.GetAllBrokers();

        if (resp.IsSuccessful)
            return View(resp.Value);
        else
        {
            _logger.LogError("Failed to retrieve brokers: {Error}", resp.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve brokers");
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> BrokerDetails(int id)
    {
        var response = await _brokerService.GetBrokerById(id);

        if (response.IsSuccessful)
            return View(response.Value);
        
        else
        {
            _logger.LogError("Failed to retrieve broker: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve broker");
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBrokersByType([FromQuery] BrokerRole brokerType)
    {
        _logger.LogInformation("BrokerType: {BrokerType}", brokerType);

        var resp = await _brokerService.GetBrokersByType(brokerType);

        if (!resp.IsSuccessful)
        {
            ModelState.AddModelError(string.Empty, resp.Error);
            return View("Error", resp.Error);
        }

        switch (brokerType)
        {
            case BrokerRole.CorporateBroker:
                ViewData["Title"] = "Corporate Brokers";
                break;
            case BrokerRole.IndividualBroker:
                ViewData["Title"] = "Individual Brokers";
                break;
            case BrokerRole.FreelanceBroker:
                ViewData["Title"] = "Freelance Brokers";
                break;
            default:
                ViewData["Title"] = "All Brokers";
                break;
        }

        return View("Index", resp.Value);
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
            return View(nameof(CreateCorporateBroker), request);
        

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
            return View(nameof(CreateIndividualBroker), request);

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
            return View(nameof(CreateFreelanceBroker), request);

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
        var resp = await _brokerService.GetBrokerById(id);

        if (resp.IsSuccessful)
        {
            var updateRequest = new UpdateCorporateBrokerRequest
            {
                BrokerId = resp.Value.BrokerId,
                CompanyName = resp.Value.CompanyName,
                CompanyEmail = resp.Value.CompanyEmail,
                CompanyMobileNumber = resp.Value.CompanyMobileNumber,
                StreetNo = resp.Value.CompanyAddress?.StreetNo,
                StreetName = resp.Value.CompanyAddress?.StreetName,
                City = resp.Value.CompanyAddress?.City,
                State = resp.Value.CompanyAddress?.State,
                ZipCode = resp.Value.CompanyAddress?.ZipCode,
                CACRegistrationNumber = resp.Value.CACRegistrationNumber,
                StockBrokerLicense = resp.Value.StockBrokerLicense,
                DateCertified = resp.Value.DateCertified
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
    public async Task<IActionResult> UpdateCorporateBroker(UpdateCorporateBrokerRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

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
            return View(request);

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeBrokerStatus(int brokerId, AccountStatus newStatus)
    {
        try
        {
            var resp = await _brokerService.ChangeBrokerStatus(brokerId, newStatus);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Broker status updated successfully.";
            }
            else
            {
                _logger.LogError($"Failed to update broker status: {resp.TechMessage}");
                TempData["ErrorMessage"] = "Failed to update broker status.";
            }

            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Details), new { id = brokerId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while updating broker status.");
            ModelState.AddModelError(string.Empty, "An error occurred while updating broker status.");
            return View(nameof(Index)); // Or appropriate view
            //return RedirectToAction(nameof(Details), new { id = brokerId });
        }
    }
}