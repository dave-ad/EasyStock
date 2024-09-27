using EasyStocks.Service.StocksServices;

namespace EasyStocks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;


    public StockController(IStockService stockService, ILogger<StockController> logger)
    {
        _stockService = stockService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for stock request: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _stockService.Create(request);

            if (response.IsSuccessful)
            {
                //_logger.LogInformation("Stock created successfully.");
                //return Ok(response);
                _logger.LogInformation("Stock created successfully with ID: {StockId}", response.Value.StockId);
                return CreatedAtAction(nameof(GetStockById), new { stockId = response.Value.StockId }, response);
            }

            _logger.LogWarning("Failed to create stock. Error: {Error}. Technical Message: {TechMessage}", response.Error, response.TechMessage);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while creating stock: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
    {
        try
        {
            var response = await _stockService.GetAll(query);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Retrieved all stocks successfully.");
                return Ok(response.Value);
            }

            _logger.LogWarning("Failed to retrieve stocks: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while retrieving all stocks: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpGet("{stockId}")]
    public async Task<IActionResult> GetStockById(int stockId)
    {
        try
        {
            var response = await _stockService.GetStockById(stockId);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Retrieved stock details for ID: {StockId} successfully.", stockId);
                return Ok(response);
            }

            _logger.LogWarning("Failed to retrieve stock: {Error}", response.Error);
            return NotFound(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while retrieving stock with ID {StockId}: {Message}", stockId, ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for update stock request.");
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _stockService.UpdateStock(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Stock updated successfully for ID: {StockId}.", request.StockId);
                return Ok(response);
            }

            _logger.LogWarning("Failed to update stock: {Error}", response.Error);
            return BadRequest(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while updating stock: {Message}", ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpDelete("{stockId}")]
    public async Task<IActionResult> DeleteStock(int stockId)
    {
        try
        {
            var response = await _stockService.DeleteStock(stockId);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Stock deleted successfully for ID: {StockId}.", stockId);
                return Ok(response);
            }

            _logger.LogWarning("Failed to delete stock: {Error}", response.Error);
            return NotFound(new
            {
                Error = response.Error,
                TechMessage = response.TechMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while deleting stock with ID {StockId}: {Message}", stockId, ex.Message);
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    //[HttpPost("watchlist/add/{userId}/{stockId}")]
    //public async Task<IActionResult> AddToWatchlist(int userId, int stockId)
    //{
    //    try
    //    {
    //        var response = await _stockService.AddToWatchlist(userId, stockId);

    //        if (response.IsSuccessful)
    //        {
    //            _logger.LogInformation("Stock added to watchlist successfully for UserId: {UserId}, StockId: {StockId}.", userId, stockId);
    //            return Ok(response);
    //        }

    //        _logger.LogWarning("Failed to add stock to watchlist: {Error}", response.Error);
    //        return BadRequest(new
    //        {
    //            Error = response.Error,
    //            TechMessage = response.TechMessage
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("An error occurred while adding stock to watchlist: {Message}", ex.Message);
    //        return StatusCode(500, "Internal server error. Please try again later.");
    //    }
    //}

    //[HttpPost("watchlist/remove/{userId}/{stockId}")]
    //public async Task<IActionResult> RemoveFromWatchlist(int userId, int stockId)
    //{
    //    try
    //    {
    //        var response = await _stockService.RemoveFromWatchList(userId, stockId);

    //        if (response.IsSuccessful)
    //        {
    //            _logger.LogInformation("Stock removed from watchlist successfully for UserId: {UserId}, StockId: {StockId}.", userId, stockId);
    //            return Ok(response);
    //        }

    //        _logger.LogWarning("Failed to remove stock from watchlist: {Error}", response.Error);
    //        return NotFound(new
    //        {
    //            Error = response.Error,
    //            TechMessage = response.TechMessage
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("An error occurred while removing stock from watchlist: {Message}", ex.Message);
    //        return StatusCode(500, "Internal server error. Please try again later.");
    //    }
    //}

    //[HttpGet("watchlist/{userId}")]
    //public async Task<IActionResult> GetWatchlist(int userId)
    //{
    //    try
    //    {
    //        var response = await _stockService.GetWatchlist(userId);

    //        if (response.IsSuccessful)
    //        {
    //            _logger.LogInformation("Retrieved watchlist for UserId: {UserId} successfully.", userId);
    //            return Ok(response);
    //        }

    //        _logger.LogWarning("Failed to retrieve watchlist: {Error}", response.Error);
    //        return BadRequest(new
    //        {
    //            Error = response.Error,
    //            TechMessage = response.TechMessage
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("An error occurred while retrieving watchlist for UserId {UserId}: {Message}", userId, ex.Message);
    //        return StatusCode(500, "Internal server error. Please try again later.");
    //    }
    //}

    //[HttpPost("buy")]
    //public async Task<IActionResult> BuyStock([FromBody] BuyStockRequest request)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        _logger.LogWarning("Invalid model state for buy stock request.");
    //        return BadRequest(ModelState);
    //    }

    //    try
    //    {
    //        var response = await _stockService.BuyStock(request);

    //        if (response.IsSuccessful)
    //        {
    //            _logger.LogInformation("Stock purchased successfully for UserId: {UserId}, StockId: {StockId}.", request.UserId, request.StockId);
    //            return Ok(response);
    //        }

    //        _logger.LogWarning("Failed to purchase stock: {Error}", response.Error);
    //        return BadRequest(new
    //        {
    //            Error = response.Error,
    //            TechMessage = response.TechMessage
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("An error occurred while purchasing stock: {Message}", ex.Message);
    //        return StatusCode(500, "Internal server error. Please try again later.");
    //    }
    //}
}