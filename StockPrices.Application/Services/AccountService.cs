using StockPrices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPrices.Infrastructure;

namespace StockPrices.Application.Services
{
    public class AccountService
    {       
        public void RegisterUser(UserDto user)
        {
            var newUser = new User()
            {
                Email = user.Email,
                Password = user.Password,
            };        
        }
    }
}
