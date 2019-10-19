using System;
using System.Threading;

namespace StonkBois
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();
        public static bool Bought = false; /* Find some kind of solution to write this in while */
        public static int BasicbuyPrice = 0, BasicsellPrice = 0, BasicProfit = 0;

        //public static int AdvanceBuyPrice = 0, AdvanceSellPrice = 0, AdvancedProfit = 0;

        static void Main(string[] args)
        {
            while (true) 
            {
                int[] marketData = GetMarketData();
                int numElements = marketData.Length; /* Lenth = 300 */

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

                //Console.WriteLine("FirstPrice: " + firstPrice + " SecoundPrice: " + lastPrice);

                BasicAlgoritme(firstPrice, lastPrice, numElements, marketData);

                //MaxMinAlgoritme(numElements, marketData);
            }
        }

        static void BasicAlgoritme(int firstPrice, int lastPrice, int numElements, int[] marketData)
        {
            int peak = 0, dip = 100000;

            for (int i = 1; i < numElements; i++)
            {
                if (peak < marketData[i - 1])
                {
                    peak = marketData[i - 1];
                }
                if (dip > marketData[i - 1])
                {
                    dip = marketData[i - 1];
                }
            }

            if (!Bought)
            {
                if (firstPrice < lastPrice && firstPrice < dip)
                {
                    // The price has risen from the first to the last data point, 
                    // so the trend is rising - buy!
                    
                    Bought = true;
                    BasicbuyPrice = lastPrice;
                    Buy();
                    BasicProfit += BasicsellPrice - BasicbuyPrice;
                }
            }
            else
            {
                if (firstPrice > lastPrice && firstPrice > peak)
                {
                    // The price has fallen from the first to the last data point, 
                    // so the trend is falling - sell!

                    Bought = false;
                    BasicsellPrice = lastPrice;
                    Sell();
                    BasicProfit += BasicsellPrice - BasicbuyPrice;
                }
            }
           
            //Console.WriteLine("Basic Algoritme:       " + "Buy: " + BasicbuyPrice + " Sell: " + BasicsellPrice + " Profit: " + BasicProfit);
        }

        /*static void MaxMinAlgoritme(int numElements, int[] marketData)
        {
            int lastPrice = 0;
            int peak = 0, dip = 100000;

            for (int i = 1; i < numElements; i++)
            {
                if (peak < marketData[i - 1])
                {
                    peak = marketData[i - 1];
                }
                if (dip > marketData[i - 1])
                {
                    dip = marketData[i - 1];
                }
            }
            // System.Console.WriteLine(peak);
            // System.Console.WriteLine(dip);
            
            if (!Bought)
            {
                if (lastPrice < dip)
                {
                    // The price has risen from the first to the last data point, 
                    // so the trend is rising - buy!
                    
                    Bought = true;
                    AdvanceBuyPrice = lastPrice;
                    Buy();
                    AdvancedProfit += AdvanceSellPrice - AdvanceBuyPrice;
                }
            }
            else
            {
                if (lastPrice > peak)
                {
                    Bought = false;
                    AdvanceSellPrice = lastPrice;
                    Sell();
                    AdvancedProfit += AdvanceSellPrice - AdvanceBuyPrice;
                }
            }

            //Console.WriteLine("Advanced Algoritme:    " + "Buy: " + AdvanceBuyPrice + " Sell: " + AdvanceSellPrice + " Profit: " + AdvancedProfit);
        }*/

































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
