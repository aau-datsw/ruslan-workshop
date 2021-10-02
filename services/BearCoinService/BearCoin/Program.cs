using System;
using System.Threading;

namespace BearCoin
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

                int firstPrice = marketData[0];  // Get the first price 
                int lastPrice = marketData[numElements-1];  // Get the last price

                /* Klassificering af data */
                int[] rising = new int[numElements];
                int[] descending = new int[numElements];
                for(int i = 0; i < numElements-1; i++)
                {
                    if(marketData[i+1] >= marketData[i])//1 er en placeholder
                    {
                        rising[i] = marketData[i];
                    
                    }
                    else if(marketData[i+1] <= marketData[i])
                    {
                        descending[i] = marketData[i];
                    }
                }
                int dlength = 0;
                int rlength = 0;
                dlength = descending.Length;
                rlength = rising.Length;
                /* Klassificering af "drastisk spring" */
                double constant = 0;
                int max = 0;
                int min = 0;
                for(int j = 0; j <= numElements-1; j++)
                {
                    max = marketData[0];
                    min = marketData[0];
                    if(max < marketData[j])
                    {
                        max = marketData[j];
                    }
                    if(min > marketData[j])
                    {
                        min = marketData[j];
                    }
                    constant = (max - min) * 0.3;
                }

                int avgfull = 0;
                int avg10  = 0;
                int len10 = 0;

                for(int g = 0; g <= numElements-1; g++)
                {
                    avgfull += marketData[g];
                }
                avgfull = avgfull/numElements;
                for(int h = numElements-1; h>numElements-(numElements/10); h--)
                {
                    avg10 += marketData[h];
                }
                len10 = numElements/10;
                avg10 = avg10/len10;
                if((marketData[numElements-2] == descending[dlength-1] && marketData[numElements-1] == rising[rlength-1]) || marketData[numElements-2] >= marketData[numElements-1]-constant)
                {
                    if(avgfull<avg10)
                    {
                        Buy();
                    }
                }
                if((marketData[numElements-2] == rising[rlength-1] && marketData[numElements-1] == descending[dlength-1]) || marketData[numElements-2] >= marketData[numElements-1]+constant)
                {
                    if(avgfull>avg10)
                    {
                        Sell();
                    }
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
