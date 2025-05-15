using Microsoft.AspNetCore.Mvc;
using StockPrices.Infrastructure.Entities;

namespace StockPrices.Controllers
{
    [Route("api/StockPrices")]
    public class StockPricesController : Controller
    {
        private readonly StockPricesDbContext dbContext;
        public StockPricesController(StockPricesDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public ActionResult<IEnumerable<StockPrice>> GetAll()
        {
            var stockPrices = dbContext.StockPrices.Take(100).ToList();
            return Ok(stockPrices);
        }
    }
}
