using System;
using System.Collections.Generic;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public interface IMarketGenerator
    {
        IEnumerable<(int x, int y)> GenerateMarketChanges(int from, int to);
    }
}