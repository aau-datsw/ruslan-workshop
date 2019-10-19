using System;
using System.Threading;

namespace KauKlanen
{
    public struct DataSet {
        public int minimumIndex;
        public int maximumIndex;
        public int average;
    }
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

                int lastPrice = marketData[numElements - 1];  // Get the last price

                DataSet curData = new DataSet();
                curData.minimumIndex = FindMinimumIndexBetween(marketData, numElements-30, numElements-1); 
                curData.maximumIndex = FindMaximumIndexBetween(marketData, numElements-30, numElements-1);
                curData.average = AverageOf(marketData[curData.minimumIndex], marketData[curData.maximumIndex]);

                DataSet prevData = new DataSet();
                prevData.minimumIndex = FindMinimumIndexBetween(marketData, numElements-60, numElements-31); 
                prevData.maximumIndex = FindMaximumIndexBetween(marketData, numElements-60, numElements-31);
                prevData.average = AverageOf(marketData[prevData.minimumIndex], marketData[prevData.maximumIndex]);

                if(curData.average > prevData.average){
                    Buy();
                } else if (curData.average < prevData.average){
                    Sell();
                }

                /* Brams løsning
                if (firstPrice < lastPrice)
                {
                    // The price has risen from the first to the last data point, 
                    // so the trend is rising - buy!
                    Buy();
                }
                else if (firstPrice > lastPrice)
                {
                    // The price has fallen from the first to the last data point, 
                    // so the trend is falling - sell!
                    Sell();
                }*/

                /* Naiv løsning
                  slope currentSlope = slope.UP;
                    Console.WriteLine(marketData);
                    if (lastPrice > point2)
                    {
                        currentSlope = slope.UP;
                        Buy();
                    }
                    else if (lastPrice < point2)
                    {
                        currentSlope = slope.DOWN;
                        Sell();
                    }
                
                 */


            }
        }

        static int FindMinimumIndexBetween(int[] data, int start, int slut){

            int min = Int32.MaxValue;
            int minimumIndex = -1;

            for (int i = 0; i < slut-start; i++)
            {
                if(data[start+i] < min){
                    minimumIndex = start + i;
                    min = data[start+i];
                }
            }

            return minimumIndex;
        }
        static int FindMaximumIndexBetween(int[] data, int start, int slut){
            int max = 0;
            int maximumIndex = -1;

            for (int i = 0; i < slut-start; i++)
            {
                if(data[start+i] > max){
                    maximumIndex = start + i;
                    max = data[start+i];
                }
            }

            return maximumIndex;
        }
        static int AverageOf(int a, int b){
            return (a+b)/2;
        }

        static int[] GetMarketData()
        {
            // Wait for 5 seconds (don't kill the server)
            Thread.Sleep(5000);
            GroupInfo info = _stonks.GetInfo();

            // Determine the timespan you want info within (this is the last 5 minutes)
            DateTime to = DateTime.Now;
            DateTime from = DateTime.Now - TimeSpan.FromMinutes(5);


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