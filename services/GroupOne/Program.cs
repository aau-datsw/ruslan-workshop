using System;
using System.Threading;

namespace GroupOne
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            while (true) 
            {
                // Wait for 10 seconds or so 
                Thread.Sleep(2000);

                DateTime to = DateTime.Now;
                DateTime from = to - TimeSpan.FromMinutes(5);

                int[] marketData = _stonks.GetMarketData(from, to);
                GroupInfo info = _stonks.GetInfo();

                // Trump doesn't care about prices, his infinite genius is enough
                int choice = new Random().Next(3);
                switch (choice)
                {
                    case 0: 
                        Buy();
                        break;
                    case 1:
                        Sell();
                        break;
                    case 2: 
                        break;
                }
            }
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
