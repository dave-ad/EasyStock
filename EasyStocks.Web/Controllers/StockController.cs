namespace EasyStocks.Web.Controllers;

public class StockController : Controller
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;

    public StockController(IStockService stockService, ILogger<StockController> logger)
    {
        _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        var response = await _stockService.GetAllStocks();

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
    public ActionResult AddStock()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStock(CreateStockRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(AddStock), request);
        }

        try
        {
            ServiceResponse<StockIdResponse> resp = await _stockService.CreateStock(request);

            if (resp.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Stock added successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Failed to add stock: {Message}", resp.TechMessage);
                ModelState.AddModelError(string.Empty, "Failed to add stock");
                return View(nameof(AddStock), request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while adding new stock.");
            ModelState.AddModelError(string.Empty, "An error occurred while adding new stock.");
            return View(nameof(AddStock), request);
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteStock(int id)
    {
        try
        {
            var response = await _stockService.DeleteStock(id);

            if (response.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Stock deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete stock: {response.Error}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while deleting stock.");
            TempData["ErrorMessage"] = "An error occurred while deleting stock.";
        }

        return RedirectToAction(nameof(Index));
    }
}