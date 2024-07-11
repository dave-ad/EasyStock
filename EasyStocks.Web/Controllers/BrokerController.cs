﻿namespace EasyStocks.Web.Controllers;

public class BrokerController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<BrokerController> _logger;
    private readonly IBrokerService _brokerService;

    public BrokerController(SignInManager<User> signInManager, ILogger<BrokerController> logger, IBrokerService brokerService)
    {
        _signInManager = signInManager;
        _brokerService = brokerService;
        _logger = logger;
    }

    public ActionResult Index()
    {
        ViewBag.BrokerCreatedMessage = "Account creation Successfully 👍";
        return View();
    }

    public async Task<IActionResult> GetAllBrokers()
    {
        var response = await _brokerService.GetAllBrokers();

        if (response.IsSuccessful)
        {
            return View(response.Value); // Assuming you have a view to display stocks
        }
        else
        {
            _logger.LogError("Failed to retrieve stocks: {Error}", response.Error);
            ModelState.AddModelError(string.Empty, "Failed to retrieve stocks");
            return View(); // Handle error scenario in the view
        }
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
}