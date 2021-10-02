using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ButtCoin
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            double average = 0;
            int i = 0;
            LinkedList<double> averagelist = new LinkedList<double>();
            while (true)
            {
                // Gettin the market data
                int[] marketData = GetMarketData();
                int numElements = marketData.Length;

                i++;
                if (i == 0)
                {

                    averagelist.AddFirst(marketData.Average());

                }
                else if(i == Convert.ToInt32(numElements/2))
                {
                    i = 0;
                }
                average = averagelist.Average();
                double diffence = average * 0.1;

                Console.WriteLine("Lol");

                if (average - diffence >= marketData[0])
                {
                    // If the price is a percent smaller than the average price
                    // But ??
                    Console.WriteLine("Buy");
                    Buy();
                }
                else if (average + diffence <= marketData[0])
                {
                    Console.WriteLine("Sell");
                    Sell();
                }
            }
        }

































        static int[] GetMarketData()
        {
            // Wait for some time (don't kill the server)
            Thread.Sleep(Environment.GetEnvironmentVariable("RUSLAN_API_PORT") == null ? 5000 : 10000);
            GroupInfo info = _stonks.GetInfo();

            // Determine the timespan you want info within (this is the last 5 minutes)
            DateTime to = DateTime.Now;
            DateTime from = to - TimeSpan.FromMinutes(5);


            // Get the market data
            return _stonks.GetMarketData(from, to);
        }

        static void Buy()
        {
            try
            {
                _stonks.Buy();
                Console.WriteLine("Bought Ligma Inc.!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Sell()
        {
            try
            {
                _stonks.Sell();
                Console.WriteLine("Sold Ligma Inc.!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
