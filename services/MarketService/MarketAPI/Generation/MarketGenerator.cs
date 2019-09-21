using System;
using System.Collections.Generic;
using System.Linq;
using MarketAPI.Models;

namespace MarketAPI.Generation
{
    public class MarketGenerator : IMarketGenerator
    {
        private const int NUM_OCTAVES = 10; 
        private const double AMPLITUDE = 128;
        private const double WAVELENGTH = 128;
        public Company Company { get; set; }

        public MarketGenerator(Company company)
        {
            Company = company;
        }
        public IEnumerable<(int x, int y)> GenerateMarketChanges(int from, int to)
        {
            // Determine hyperparameters from the company volatility. The more volatile the company,
            // the shorter the wavelength, the larger the amplitude.

            // Generate the price points, combine with the timestamps so they can be projected onto a 
            // 2-d graph.
            var noiseParams = GetNoiseParams(Company);
            var pricePoints = new PerlinNoise(Company.Id).GenerateNoise(AMPLITUDE * noiseParams.Item1, WAVELENGTH * noiseParams.Item2, NUM_OCTAVES, to - from).Select(o => (int)o);
            return from.Range(to - from).Zip(pricePoints);
        }

        private (double, double) GetNoiseParams(Company company)
        {
            switch (company.Volatility)
            {
                case 0:  // Slow company
                    return (12.0, 12.0);
                case 1:  // Normal company
                    return (24.0, 24.0);
                case 2:  // Volatile company
                    return (256.0, 256.0);
                default: 
                    throw new ArgumentException($"Volatility must be a value between {{0, 2}}, but was {company.Volatility}");
            }
        }
    }
}