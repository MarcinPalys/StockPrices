using Microsoft.AspNetCore.Mvc;
using StockPrices.Core.Entities;
using StockPrices.Infrastructure;

namespace StockPrices.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly StockPricesDbContext _stockPricesDbContext;
        public AccountController(StockPricesDbContext stockPricesDbContext)
        {
            _stockPricesDbContext = stockPricesDbContext;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] UserDto user)
        {
            var User = new User()
            {
                Email = user.Email,
                Password = user.Password,
            };
            _stockPricesDbContext.Users.Add(User);
            _stockPricesDbContext.SaveChanges();

            return Ok();
        }
    }
}
