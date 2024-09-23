namespace EasyStocks.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockService _stockService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IStockService stockService, ILogger<HomeController> logger)
        {
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var response = await _stockService.GetAll();

        //    if (response.IsSuccessful)
        //    {
        //        return View(response.Value); // Assuming you have a view to display stocks
        //    }
        //    else
        //    {
        //        _logger.LogError("Failed to retrieve stocks: {Error}", response.Error);
        //        ModelState.AddModelError(string.Empty, "Failed to retrieve stocks");
        //        return View(); // Handle error scenario in the view
        //    }
        //}

        public IActionResult PageViews()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
