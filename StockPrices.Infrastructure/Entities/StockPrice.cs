using System;
using System.Collections.Generic;

namespace StockPrices.Infrastructure.Entities;

public partial class StockPrice
{
    public string? Symbol { get; set; }

    public string? Date { get; set; }

    public string? Open { get; set; }

    public string? High { get; set; }

    public string? Low { get; set; }

    public string? Close { get; set; }

    public string? Volume { get; set; }
}
