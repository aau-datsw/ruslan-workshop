using System;
using System.Threading;

namespace StonkBois
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            while (true) 
            {
                int[] marketData = GetMarketData();
                int numElements = marketData.Length; /* Lenth = 300 */
                
                int currentMoney = 100000;
                int stockCount = 0;
                int buyPrice = 0, sellPrice = 0, profit = 0;
                bool Bought = false;

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

                /*for (int i = 0; i < numElements; i++)
                {
                    Console.WriteLine(marketData[i]);
                }*/

                //BasicAlgorithem(firstPrice, lastPrice, currentMoney, stockCount, buyPrice, sellPrice, profit, Bought);

                if (firstPrice < lastPrice)
                {
                    // The price has risen from the first to the last data point, 
                    // so the trend is rising - buy!
                    
                    Buy();
                } 
                
                if (firstPrice > lastPrice)
                {
                    // The price has fallen from the first to the last data point, 
                    // so the trend is falling - sell!

                    Sell();
                }
            }
        }

        static void BasicAlgorithem(int firstPrice, int lastPrice, int currentMoney, int stockCount, int buyPrice, int sellPrice, int profit, bool Bought)
        {
            Console.WriteLine("FirstPrice: " + firstPrice + " SecoundPrice: " + lastPrice);
            if (firstPrice < lastPrice && !Bought)
            {
                // The price has risen from the first to the last data point, 
                // so the trend is rising - buy!
                
                Console.WriteLine("Buy");
                Bought = true;
                
                Buy();
            } 
            
            if (firstPrice > lastPrice && Bought)
            {
                // The price has fallen from the first to the last data point, 
                // so the trend is falling - sell!

                Console.WriteLine("Sell");
                Bought = false;

                Sell();
            }/*
            Console.WriteLine("FirstPrice: " + firstPrice + " SecoundPrice: " + lastPrice);
            Console.WriteLine("Money: " + currentMoney + " StockCount: " + stockCount);
            Console.WriteLine("Buy: " + buyPrice + " Sell: " + sellPrice + " Profit: " + profit);*/
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
