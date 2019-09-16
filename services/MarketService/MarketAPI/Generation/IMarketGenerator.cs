using System;
using System.Collections.Generic;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public interface IMarketGenerator
    {
        IEnumerable<(int x, double y)> GenerateMarketChanges(Company company, int from, int to);
    }
}