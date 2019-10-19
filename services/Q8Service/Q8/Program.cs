using System;
using System.Threading;

namespace Q8
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            int[] marketData = GetMarketData();
            int numElements = marketData.Length;
            int lastBuy = marketData[numElements-1];
            int flag = 0;
            while (true) 
            {
                /*Det er her vores kode skal skrives!!!*/
                int[] marketData = GetMarketData();
                int numElements = marketData.Length;
                
                //TODO : sorter så kun de nye data kommer over i tekstfilen!!!!  
                //skriv indholdet af marketData til en tekstfil på serveren
                /*
                TextWriter tw = new StreamWriter("STONKS.txt",true);
                if (tw.file < 10)
                {
                    foreach (int i in marketData) 
                    {
                        tw.Write(i);
                        tw.Write(',');  
                    }
                }
                else
                {
                    tw.Write(marketData[numElements-1]);
                    tw.Write(',');
                }
                */
        
        
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
                int middle = (lastBuy+50 > very_last && lastBuy-50 < very_last)
                
                if (flag == 1 && (third_last < second_last && second_last > very_last))
                {
                    Sell();
                    lastBuy = very_last;
                    flag = 0;
                }
                else if (flag == 1 && (third_last > second_last && second_last < very_last))
                {
                    Buy();
                    lastBuy = very_last;
                    flag = 0;
                }
                else if (flag == 0 && middle)
                {
                    if (lastBuy < very_last)
                    {
                        Sell();
                        flag = 0;
                    }
                    else if(lastBuy > very_last)
                    {
                        Buy();
                        flag = 0;
                    }
                }
                else if (flag == 0 && !middle)
                {
                    if (lastBuy < very_last)
                    {
                        Sell();
                        flag = 1;
                    }
                    else if(lastBuy > very_last)
                    {
                        Buy();
                        flag = 1;
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
