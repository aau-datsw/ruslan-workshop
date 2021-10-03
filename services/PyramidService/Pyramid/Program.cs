using System;
using System.Threading;

namespace Pyramid
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            while (true) 
            {
                try {
                int[] marketData = GetMarketData();
                int numElements = marketData.Length;

                // ------------------------------------------------------ // 
                //          THIS IS WHERE YOU WRITE YOUR CODE!            // 
                //                      GOOD LUCK!                        //
                // ------------------------------------------------------ //

                int slope,
                    newValue = marketData[0],
                    sumRandomSlope = 0,
                    sumRandomChange = 0;
                int[] slopeArray = new int[numElements],
                      changeArray = new int[numElements],
                      slopeArrayRandom = new int[numElements],
                      changeArrayRandom = new int[numElements];
                
                Random rnd = new Random();
                int num = 9; /*rnd.Next(1, (numElements - 1));*/

                for(int i = 0; i < numElements - 1; i++) {
                    slope = marketData[i] - marketData[i + 1];
                    slopeArray[i] = slope;
                }

                for(int i = 0; i < num; i++) {
                    slope = slopeArray[i] - slopeArray[i + 1];
                    slopeArrayRandom[i] = slope;
                    sumRandomSlope += slope;
                }

                for(int i = 0; i < (numElements - 1); i++) {
                    slope = slopeArray[i] - slopeArray[i + 1];
                    changeArray[i] = slope;
                }

                for(int i = 0; i < num; i++) {
                    slope = changeArray[i] - changeArray[i + 1];
                    changeArrayRandom[i] = slope;
                    sumRandomChange += slope;
                }

                if (sumRandomChange > 0) {
                    if (sumRandomSlope > 0) {
                        Buy();
                    }
                }

                else if (sumRandomChange < 0) {
                    if (sumRandomSlope < 0) {
                        Sell();
                    }
                }

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
                } catch (Exception e) {
                    Console.WriteLine("{0} First exception caught.", e);
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
