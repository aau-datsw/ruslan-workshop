using System;
using System.Threading;

namespace Balle
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
                int local_percentage = 50;
                int local_size = numElements * (local_percentage / 100);
                int new_percentage = 5;
                int new_size = numElements * (new_percentage / 100);
                int global_total = 0;
                foreach (int i in marketData)
                {
                    global_total += i;
                }
                float global_avg = global_total / numElements;
                int local_total = 0;
                for (int i = 0; i < local_size; i++)
                {
                    local_total += marketData[i];
                }
                float local_avg = local_total / local_size;
                
                int new_total = 0;
                for (int i = 0; i < new_size; i++)
                {
                    new_total += marketData[i];
                }
                float new_avg = new_total / new_size;
                

                // buy when price is cheap, but rising
                if (local_avg < global_avg && new_avg > local_avg)
                {
                    Buy();
                }

                // sell when price is high, but falling
                if (local_avg > global_avg && new_avg < local_avg)
                {
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
