﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPrices.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
       
        public string Symbol { get; set; }
       
        public string Text { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
