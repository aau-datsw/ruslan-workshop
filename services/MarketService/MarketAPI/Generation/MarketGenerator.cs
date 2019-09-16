using System.Collections.Generic;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public class MarketGenerator : IMarketGenerator
    {
        public IEnumerable<(float x, float y)> GetMarketChanges(Company company, int from, int to)
        {
            throw new System.NotImplementedException();
        }
    }
}