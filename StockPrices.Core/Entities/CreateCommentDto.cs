using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPrices.Core.Entities
{
    public class CreateCommentDto
    {
        public string Symbol { get; set; }
        public string Text { get; set; }
    }
}
