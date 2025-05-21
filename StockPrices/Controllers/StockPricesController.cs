using Microsoft.AspNetCore.Mvc;
using StockPrices.Infrastructure;
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
            var result = dbContext.StockPrices.Take(100).ToList();
            return Ok(result);
        }
        [HttpGet("symbol/{symbol}")]
        public ActionResult<IEnumerable<StockPrice>> GetBySymbol([FromRoute] string symbol)
        {
            var result = dbContext.StockPrices.Where(s => s.Symbol == symbol).ToList();

            if(result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("symbol/{symbol}/date/{date}")]
        public ActionResult<StockPrice> GetBySymbolAndDate([FromRoute] string symbol, [FromRoute] string date)
        {
            var result = dbContext.StockPrices.FirstOrDefault(s => s.Symbol == symbol && s.Date == date);

            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("date/{date}")]
        public ActionResult<IEnumerable<StockPrice>> GetBydDate([FromRoute] string date)
        {
            var result = dbContext.StockPrices
                .Where(d => d.Date == date)
                .ToList();

            if (result is null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("symbol/{symbol}/range")]
        public ActionResult<IEnumerable<StockPrice>> GetBySymbolAndDate([FromRoute] string symbol,[FromQuery] DateTime from,[FromQuery] DateTime to)
        {
             var result = dbContext.StockPrices
            .Where(s => s.Symbol == symbol)
            .AsEnumerable()
            .Where(s =>
             DateTime.TryParse(s.Date, out var date) &&
             date >= from && date <= to)
            .OrderBy(s => DateTime.Parse(s.Date))
            .ToList();

            if (result is null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}