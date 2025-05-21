using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPrices.Core.Entities
{
    public class FavoriteStock
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Symbol { get; set; }
    }
}
