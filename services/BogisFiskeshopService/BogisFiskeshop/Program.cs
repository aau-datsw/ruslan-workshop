using System;
using System.Threading;
using System.Linq;

namespace BogisFiskeshop
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
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
                var newestData = marketData.Skip(numElements-11).Take(10).ToArray(); 

                if(Trending(marketData))
                {
                    if (!Trending(newestData))
                    {
                        Sell();
                    }
                } else {
                    if (!Trending(newestData))
                    {
                        Buy();
                    }
                }
            }
        }




        static bool Trending(int[] marketData)
        {
            int numElements = marketData.Length;
            int avgX = (marketData.Sum()/numElements);
            int avgY = ((numElements*numElements+1)/2);
            int sum1 = 0;
            int sum2 = 0;
            for(int i = 0; i < numElements; i++){
                sum1 = sum1 + ((marketData[i]-avgX)*(i-avgY));
            }
            for(int i = 0; i < numElements; i++){
                sum2 = sum2 + (marketData[i]-avgX)^2;
            }
            if(sum1/sum2 > 0)
            {
                return true;
            } else {
                return false;
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
