using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using StockPrices.Core.Entities;
using StockPrices.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockPrices.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly StockPricesDbContext _stockPricesDbContext;
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountController(StockPricesDbContext stockPricesDbContext, AuthenticationSettings authenticationSettings)
        {
            _stockPricesDbContext = stockPricesDbContext;
            _authenticationSettings = authenticationSettings;
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

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserDto dto)
        {
            var user = _stockPricesDbContext.Users
                .FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);
            if(user is null)
            {
                return Unauthorized();
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.JwtIssuer,
                audience: _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }
    }
}
