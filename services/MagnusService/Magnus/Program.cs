using System;
using System.Linq;
using System.Threading;

namespace Magnus
{
    internal class Program
    {
        private static StonksUtils _stonks = new StonksUtils();

        private static void Main(string[] args)
        {
            try {
            while (true)
            {
                int[] marketData = GetMarketData();
                if (marketData[marketData.Length - 1] < marketData.Average()) Buy();
                else Sell();
            }
            } catch (Exception e) {
                Console.WriteLine("{0} First exception caught.", e);
            }
        }

        private static int[] GetMarketData()
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

        private static void Buy()
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

        private static void Sell()
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