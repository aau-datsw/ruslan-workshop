using System;
using System.Threading;

namespace Q8
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            while (true) 
            {
                /*Det er her vores kode skal skrives!!!*/
        
        
                /*Denne kode er skrevet ud fra forudsætningen, at hvis den er stigende (tredjesidst<andensidst<allersidst) så skal vi ikke gøre noget.
                Ligeledes med det omvendte tilfælde, hvor den er konstant faldende.
                Det er kun lige efter toppunkterne (hvor vi ved det er et toppunkt) at vi enten køber eller sælger.
                Altså, hvis tredjesidst er mindre end anden, men anden er større end første, så sælger vi.
                Ligeledes hvis tredje er større end anden, men anden er mindre end første, så køber vi.
                Det gør vores algoritme altid er 1 bagud, men til gengæld følger udviklingen.
                Dog hvis der er mange toppunkter, så vil vores algoritme SUCKS...
                */
        
                int third_last = marketData[numElements-3];  // Get the third last price
                int second_last = marketData[numElements-2]; // Get the second last price
                int very_last = marketData[numElements-1];  // Get the last price
        
                if (third_last < second_last && second_last > very_last)
                {
                    Sell();
                }
                else if (third_last > second_last && second_last < very_last)
                {
                    Buy();
                }
                else;
            }
        }

































        static int[] GetMarketData()
        {
            // Wait for some time (don't kill the server)
            Thread.Sleep(Environment.GetEnvironmentVariable("RUSLAN_API_PORT") == null ? 5000 : 10000);
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
