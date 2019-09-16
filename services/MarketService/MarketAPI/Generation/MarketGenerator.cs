using System.Collections.Generic;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public class MarketGenerator : IMarketGenerator
    {
        private const int NUM_OCTAVES = 10; 
        private const double AMPLITUDE = 128;
        private const double WAVELENGTH = 128;
        public IEnumerable<(double x, double y)> GetMarketChanges(Company company, int from, int to)
        {
            // Generate the price points, combine with the timestamps so they can be projected onto a 
            // 2-d graph.
            var pricePoints = new PerlinNoise().GenerateNoise(AMPLITUDE, WAVELENGTH, NUM_OCTAVES, to - from);

        }
    }
}