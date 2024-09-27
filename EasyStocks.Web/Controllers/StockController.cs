//using EasyStocks.Domain.Entities;

//namespace EasyStocks.Web.Controllers;

//public class StockController : Controller
//{
//    private readonly IStockService _stockService;
//    private readonly ILogger<StockController> _logger;
//    //private readonly UserManager<EasyStockUser> _userManager;

//    public StockController(IStockService stockService, ILogger<StockController> logger/*, UserManager<EasyStockUser> userManager*/)
//    {
//        _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        //_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
//    }

//    //public async Task<IActionResult> Index()
//    //{
//    //    var response = await _stockService.GetAll();

//    //    if (response.IsSuccessful)
//    //    {
//    //        return View(response.Value); // Assuming you have a view to display stocks
//    //    }
//    //    else
//    //    {
//    //        _logger.LogError("Failed to retrieve stocks: {Error}", response.Error);
//    //        ModelState.AddModelError(string.Empty, "Failed to retrieve stocks");
//    //        return View(); // Handle error scenario in the view
//    //    }
//    //}

//    [HttpGet]
//    public ActionResult AddStock()
//    {
//        return View();
//    }

//    [HttpPost]
//    public async Task<IActionResult> AddStock(CreateStockRequest request)
//    {
//        if (!ModelState.IsValid)
//        {
//            return View(nameof(AddStock), request);
//        }

//        try
//        {
//            ServiceResponse<StockResponse> resp = await _stockService.Create(request);

//            if (resp.IsSuccessful)
//            {
//                TempData["SuccessMessage"] = "Stock added successfully.";
//                return RedirectToAction(nameof(Index));
//            }
//            else
//            {
//                _logger.LogError("Failed to add stock: {Message}", resp.TechMessage);
//                ModelState.AddModelError(string.Empty, "Failed to add stock");
//                return View(nameof(AddStock), request);
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An exception occurred while adding new stock.");
//            ModelState.AddModelError(string.Empty, "An error occurred while adding new stock.");
//            return View(nameof(AddStock), request);
//        }
//    }

//    [HttpGet]
//    [Route("Stock/GetStockById/{id}")]
//    public async Task<IActionResult> GetStockById(int id)
//    {
//        var resp = await _stockService.GetStockById(id);

//        if (!resp.IsSuccessful)
//        {
//            _logger.LogError("Failed to fetch stock: {Message}", resp.Error);
//            ModelState.AddModelError(string.Empty, "Failed to fetch stock");
//            return NotFound(resp.Error);
//        }

//        return Ok(resp.Value);
//    }

//    [HttpGet]
//    public async Task<IActionResult> StockDetails(int id)
//    {
//        var resp = await _stockService.GetStockById(id);

//        if (resp.IsSuccessful)
//        {
//            return View(resp.Value);
//        }
//        else
//        {
//            _logger.LogError("Failed to retrieve stock: {Error}", resp.Error);
//            ModelState.AddModelError(string.Empty, "Failed to retrieve stock");
//            return View();
//        }
//    }

//    [HttpGet]
//    public async Task<IActionResult> UpdateStock(int id)
//    {
//        var resp = await _stockService.GetStockById(id);

//        if (resp.IsSuccessful)
//        {
//            var updateRequest = new UpdateStockRequest
//            {
//                Id = id,
//                StockTitle = resp.Value.StockTitle,
//                CompanyName = resp.Value.CompanyName,
//                StockType = resp.Value.StockType,
//                TotalUnits = resp.Value.TotalUnits,
//                PricePerUnit = resp.Value.PricePerUnit,
//                OpeningDate = resp.Value.OpeningDate,
//                ClosingDate = resp.Value.ClosingDate,
//                MinimumPurchase = resp.Value.MinimumPurchase,
//                DateListed = resp.Value.DateListed,
//                ListedBy = resp.Value.ListedBy
//            };

//            return View(updateRequest);
//        }
//        else
//        {
//            _logger.LogError("Failed to retrieve stock for update: {Error}", resp.Error);
//            ModelState.AddModelError(string.Empty, "Failed to retrieve stock for update");
//            return View();
//        }
//    }

//    [HttpPost]
//    public async Task<IActionResult> UpdateStock(UpdateStockRequest request)
//    {
//        if (!ModelState.IsValid)
//            return View(request);

//        try
//        {
//            var response = await _stockService.UpdateStock(request);
//            if (response.IsSuccessful)
//            {
//                TempData["SuccessMessage"] = "Stock updated successfully.";
//                return RedirectToAction(nameof(Index));
//            }
//            else
//            {
//                _logger.LogError("Failed to update broker: {Error}", response.Error);
//                ModelState.AddModelError(string.Empty, "Failed to update broker");
//                return View(request);
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An exception occurred while updating broker.");
//            ModelState.AddModelError(string.Empty, "An error occurred while updating broker.");
//            return View(request);
//        }
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteStock(int id)
//    {
//        try
//        {
//            var response = await _stockService.DeleteStock(id);

//            if (response.IsSuccessful)
//            {
//                TempData["SuccessMessage"] = "Stock deleted successfully.";
//            }
//            else
//            {
//                TempData["ErrorMessage"] = $"Failed to delete stock: {response.Error}";
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An exception occurred while deleting stock.");
//            TempData["ErrorMessage"] = "An error occurred while deleting stock.";
//        }

//        return RedirectToAction(nameof(Index));
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> AddToWatchlist(int stockId)
//    {
//        //var userId = await GetCurrentUserIdAsync();
//        var userId = GetCurrentUserId();

//        var response = await _stockService.AddToWatchlist(userId, stockId);

//        if (response.IsSuccessful)
//        {
//            TempData["SuccessMessage"] = "Stock added to watchlist successfully.";
//        }
//        else
//        {
//            _logger.LogError("Failed to add stock to watchlist: {Error}", response.Error);
//            TempData["ErrorMessage"] = response.Error;
//        }

//        return RedirectToAction(nameof(Index));
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> RemoveFromWatchlist(int stockId)
//    {
//        //var userId = await GetCurrentUserIdAsync();
//        var userId = GetCurrentUserId();

//        var response = await _stockService.RemoveFromWatchList(userId, stockId);

//        if (response.IsSuccessful)
//        {
//            TempData["SuccessMessage"] = "Stock removed from watchlist successfully.";
//        }
//        else
//        {
//            _logger.LogError("Failed to remove stock from watchlist: {Error}", response.Error);
//            TempData["ErrorMessage"] = response.Error;
//        }

//        return RedirectToAction(nameof(Index));
//    }

//    [HttpGet]
//    public async Task<IActionResult> Watchlist()
//    {
//        try
//        {
//            //var userId =  await GetCurrentUserIdAsync();
//            var userId =  GetCurrentUserId();

//            var response = await _stockService.GetWatchlist(userId);

//            if (response.IsSuccessful)
//            {
//                return View(response.Value); // Assuming you have a view to display the watchlist
//            }
//            else
//            {
//                _logger.LogError("Failed to retrieve watchlist: {Error}", response.Error);
//                ModelState.AddModelError(string.Empty, "Failed to retrieve watchlist");
//                return View(); // Handle error scenario in the view
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An error occurred while retrieving the watchlist.");
//            ModelState.AddModelError(string.Empty, "An error occurred while retrieving the watchlist.");
//            return View(); // Handle general error scenario in the view
//        }
//    }

//    //// Helper method to get current user id
//    //private async Task<int> GetCurrentUserIdAsync()
//    //{
//    //    var user = await _userManager.GetUserAsync(User);
//    //    if (user != null)
//    //    {
//    //        return user.Id;
//    //    }
//    //    // Example: return User.Identity.GetUserId<int>();
//    //    throw new UnauthorizedAccessException("User is not authenticated.");
//    //    //return 1; // Placeholder for example
//    //}

//    // Helper method to get current user id
    
//    private int GetCurrentUserId()
//    {
//        return 1; // Placeholder for example
//    }

//    // For when JWT is implemented
//    //private int GetCurrentUserId()
//    //{
//    //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//    //    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
//    //    {
//    //        return userId;
//    //    }

//    //    throw new UnauthorizedAccessException("User is not authenticated.");
//    //}

//    [HttpGet]
//    public async Task<IActionResult> BuyStock(int stockId)
//    {
//        var stockResponse = await _stockService.GetStockById(stockId);

//        if (!stockResponse.IsSuccessful)
//        {
//            _logger.LogError("Failed to retrieve stock for purchase: {Error}", stockResponse.Error);
//            TempData["ErrorMessage"] = "Unable to retrieve stock details. Please try again.";
//            return RedirectToAction(nameof(Index));
//        }

//        var userId = GetCurrentUserId(); // Get the current user's ID

//        var viewModel = new BuyStockRequest
//        {
//            StockId = stockId,
//            UserId = userId,
//            StockTitle = stockResponse.Value.StockTitle,
//            CompanyName = stockResponse.Value.CompanyName,
//            PricePerUnit = stockResponse.Value.PricePerUnit,
//            TotalUnits = stockResponse.Value.TotalUnits
//        };
//        return View(viewModel);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> BuyStock(BuyStockRequest request)
//    {
//        if (!ModelState.IsValid)
//        {
//            return View(request); // Return the view with the model if validation fails
//        }

//        try
//        {
//            var userId = GetCurrentUserId(); // Get the current user's ID

//            var buyStockRequest = new BuyStockRequest
//            {
//                StockId = request.StockId,
//                UserId = userId,
//                UnitPurchase = request.UnitPurchase
//            };

//            var resp = await _stockService.BuyStock(buyStockRequest);

//            if (resp.IsSuccessful)
//            {
//                TempData["SuccessMessage"] = "Stock purchased successfully.";
//                return RedirectToAction(nameof(Index)); // Redirect to the stock list or another appropriate page
//            }
//            else
//            {
//                _logger.LogError("Failed to purchase stock: {Error}", resp.Error);
//                ModelState.AddModelError(string.Empty, resp.Error);
//                return View(request); // Return the view with error message
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An exception occurred while purchasing stock.");
//            ModelState.AddModelError(string.Empty, "An error occurred while purchasing stock.");
//            return View(request); // Return the view with error message
//        }
//    }

//}