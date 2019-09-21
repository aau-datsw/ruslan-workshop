using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketAPI.Generation
{
    public class PerlinNoise
    {
        // Parameters for generation the linear congruential noise
        private const int M = 424967296;
        private const int A = 1664525;
        private const int C = 1;

        private double Z; 

        public PerlinNoise(int seed)
        {
            var r = new Random(seed);
            Z = Math.Floor(r.NextDouble() * M);
        }

        // Consider this a pseudo-random number generator. It's not perfect
        // but it's fine for our noise-generating purposes.
        private double Next()
        {
            Z = (A * Z + C) % M;
            return Z / M - 0.5;
        }

        // Cosine interpolation between a and b by a percentage.
        // Generates smoother interpolations than its linear counterpart.
        // See http://paulbourke.net/miscellaneous/interpolation/ 
        // for an overview of different interpolations.
        private double Interpolate(double a, double b, double rate)
        {
            var angle = rate * Math.PI;
            var percentage = (1 - Math.Cos(angle)) * 0.5;

            return a * (1 - percentage) + b * percentage;
        }

        // Generates a single layer of noise within a range (width). 
        // Change the amplitude and wavelengths to adjust the rate 
        // at which the stock changes. Combine several layers to 
        // better emulate reality.
        private List<double> Perlin(double amplitude, double waveLength, int width)
        {
            var x = 0.0;
            var frequency = 1 / waveLength;
            var (a, b) = (Next(), Next());
            var noise = new List<double>();

            while (x < width)
            {
                if (x % waveLength == 0)
                {
                    a = b;
                    b = Next();
                    noise.Add(a * amplitude);
                }
                else 
                {
                    noise.Add(Interpolate(a, b, (x % waveLength) / waveLength) * amplitude);
                }

                x++;
            }

            return noise;
        }

        private List<List<double>> GenerateNoiseLayers(double amplitude, double waveLength, int octaves, double divisor, int width)
        {
            var layers = new List<List<double>>();
            for (int i = 0; i < octaves; i++)
            {
                layers.Add(Perlin(amplitude, waveLength, width));
                amplitude /= divisor;
                waveLength /= divisor;
            }

            return layers;
        }

        private List<double> CombineLayers(List<List<double>> noiseLayers)
        {
            var noise = new List<double>();
            var length = noiseLayers.First().Count;
            for (int i = 0; i < length; i++)
            {
                var aggregate = 0.0;
                for (int j = 0; j < noiseLayers.Count; j++)
                {
                    aggregate += noiseLayers[j][i];
                }

                noise.Add(aggregate);
            }

            return noise;
        }

        public List<double> GenerateNoise(double amplitude, double waveLength, int octaves, int width)
        {
            return CombineLayers(GenerateNoiseLayers(amplitude, waveLength, octaves, 2, width));
        }


    }
}