using System;
using System.Collections.Generic;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public interface IMarketGenerator
    {
        IEnumerable<(double x, double y)> GetMarketChanges(Company company, int from, int to);
    }
}