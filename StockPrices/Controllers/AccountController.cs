using Microsoft.AspNetCore.Mvc;
using StockPrices.Core.Entities;

namespace StockPrices.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] UserDto user)
        {
            
            return Ok("User registered successfully");
        }
    }
}
