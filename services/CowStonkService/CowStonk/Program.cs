using System;
using System.Threading;

namespace CowStonk
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

                int local_size = (int) numElements / 100 * 10;
                int dip_size = (int) numElements / 100 * 10;
                int critical_point = marketData[dip_size];
                int dip_check = 0;
                int total = 0;
                int local_total = 0;
                int current_price = marketData[0];

                foreach (int i in marketData)
                {
                    total += i;
                }

                for (int i = 0; i < local_size; i++)
                {
                    int value = marketData[i];
                    local_total += value;
                }

                for (int i = 0; i < dip_size; i++)
                {
                    int value = marketData[i];
                    dip_check += value;
                }

                float total_avg = total / numElements;
                float local_avg = local_total / local_size;
                float dip_check_avg = dip_check / dip_size;

                if (local_avg > total_avg)
                {
                    if ( critical_point > dip_check_avg)
                    {
                        Sell();
                    }
                }
                
                else if (local_avg < total_avg)
                {
                    if ( critical_point < dip_check_avg)
                    {
                        Buy();
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
