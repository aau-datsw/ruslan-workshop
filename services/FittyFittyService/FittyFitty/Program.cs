using System;
using System.Threading;

namespace FittyFitty
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            while (true) 
            {
               
            Random rd = new Random();

            int rand_num = rd.Next(0, 2);

            if (rand_num == 1)
            {
                Sell();
            } else
            {
                Buy();
            }
            }
        }

































        static int[] GetMarketData()
        {
            // Wait for some time (don't kill the server)
            Thread.Sleep(Environment.GetEnvironmentVariable("RUSLAN_API_PORT") == null ? 5000 : 10000);
            GroupInfo info = _stonks.GetInfo();

            // Determine the timespan you want info within (this is the last 5 minutes)
            DateTime to = Environment.GetEnvironmentVariable("RUSLAN_API_PORT") == null ? DateTime.Now - TimeSpan.FromDays(2) : DateTime.Now;
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
