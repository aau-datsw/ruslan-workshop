using System;
using System.Threading;

namespace GroupOne
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) 
            {
                // Wait for 10 seconds or so 
                Thread.Sleep(500);
                System.Console.WriteLine("Hello");

                bool shouldSell = false, shouldBuy = false;
                int[] marketData = Stonks.GetMarketData(DateTime.Now - TimeSpan.FromMinutes(5), DateTime.Now);
                for (int i = 0; i < marketData.Length; i++)
                {
                    // Do work
                }

                if (shouldBuy)
                {
                    Stonks.Buy(10);
                } 
                else if (shouldSell)
                {
                    Stonks.Sell(10);
                }
                else 
                {
                    // Do nothing, just wait
                }
            }
        }
    }
}
