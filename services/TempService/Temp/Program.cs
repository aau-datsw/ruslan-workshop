using System;
using System.Threading;

namespace Temp
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            int prevPrice = 0;
            int price = 0;
            int[] TempMarketData = new int[] {};
            int[] prevMarketData = TempMarketData ?? null;
            while (true) 
            {
                TempMarketData = GetMarketData();
                price = TempMarketData[TempMarketData.Length - 1];


                //List<int> AllMarketData = new List<int>();


                //Find the elements after the elements in another array.
                //GetOnlyNewData();

                double sum = 0;

                for (int i = 0; i < TempMarketData.Length - 1; i++)
                {
                    sum += TempMarketData[i];
                }

                double average = sum / TempMarketData.Length;
                
                if (prevPrice < average && price > average)
                {
                    Buy();
                }

                if (prevPrice > average && price > average)
                {
                    Buy();  
                }

                if (prevPrice > average && price < average)
                {
                    Sell();
                }

                if (prevPrice < average && price < average)
                {
                    Sell();
                }
                Console.WriteLine(DateTime.Now);
                Console.WriteLine("Price: {0}, Average: {1}, Prev price: {2}.", price, average, prevPrice);


                prevPrice = price;
            }
        }

        /*
        List<int> GetOnlyNewData (int[] oldArray, int[] newArray)
        {
            List<int> result = new List<int>();
            int offset = 0;
            bool offsetFound = false;
            while (offsetFound == null)
            {
                //if (oldArray[oldArray.Length - 1] == )
            }
            return result;
        }
        */







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
