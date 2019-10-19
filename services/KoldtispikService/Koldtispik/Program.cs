using System;
using System.Threading;

namespace Koldtispik
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            int threshold = 0;
            int pointsInTrend = 10;
            while (true) 
            {
                int[] marketData = GetMarketData();
                int numElements = marketData.Length;

                // ------------------------------------------------------ // 
                //          THIS IS WHERE YOU WRITE YOUR CODE!            // 
                //                      GOOD LUCK!                        //
                // ------------------------------------------------------ //

                // ------------------------------------------------------ //
                //          THE FOLLOWING IS EXAMPLE CODE - IT            // 
                //          CHECKS THE FIRST AND LAST PRICES IN           // 
                //          THE MARKET DATA AND:                          // 
                //                                                        // 
                //          FIRST < LAST      ---->      BUY              // 
                //          FIRST > LAST      ---->      SELL             // 
                //          FIRST = LAST      ---->      STAY             //
                //                                                        // 
                //          FEEL FREE TO REPLACE WITH YOUR OWN!           //
                //                                                        // 
                // ------------------------------------------------------ //

                int firstPrice = marketData[0];  // Get the first price 
                int lastPrice = marketData[numElements-1];  // Get the last price
                int secondLastPrice = marketData[numElements-2];
                int thirdLastPrice = marketData[numElements-3];
                int fourthLastPrice = marketData[numElements-4];
                
                //int sum = Sum(marketData);
                //int Trend = (lastPrice-secondLastPrice + secondLastPrice-thirdLastPrice + thirdLastPrice - fourthLastPrice)/3;
                float Trend = 0;
                
                for (int i = numElements-1; i > numElements - pointsInTrend ;i--)
                {
                    Trend += marketData[i] - marketData[i - 1];
                }
                Trend = Trend / pointsInTrend;
                
                int smallTrend = (lastPrice-secondLastPrice);
                
                if (0 > Trend && 0 < smallTrend)
                {
                    // The price has risen from the first to the last data point, 
                    // so the trend is rising - buy!
                    Buy();
                    threshold = lastPrice;
                }
                else if (Trend > 0 && smallTrend < 0 && lastPrice > threshold)
                {
                    // The price has fallen from the first to the last data point, 
                    // so the trend is falling - sell!
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
