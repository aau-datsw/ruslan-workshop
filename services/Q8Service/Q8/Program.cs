using System;
using System.Threading;

namespace Q8
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils();

        static void Main(string[] args)
        {
            int[] marketDataInit = GetMarketData();
            int numElementsInit = marketDataInit.Length;
            int lastBuy = marketDataInit[numElementsInit-1];
            int flag = 0;
            while (true) 
            {
                /*Det er her vores kode skal skrives!!!*/
                int[] marketData = GetMarketData();
                int numElements = marketData.Length;
                
                //TODO : sorter så kun de nye data kommer over i tekstfilen!!!!  
                //skriver indholdet af marketData til en tekstfil på serveren
                //Der kan testes for om den virker ved at kører dotnet run i command prompten, indsæt evt. print funktioner for at teste.
                //Sørg for at en evt. commited STONKS.txt fil er tom.
                /*
                TextWriter tw = new StreamWriter("STONKS.txt",true);
                TextReader tr = StreamReader(STONKS.txt);
                if (tr)
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
        
  
                /*Dette er koden for vores køb og salg algoritme.
                Den basere sig på princippet, at den først tjekker for om vi er i en trend (flag=1) og om trenden er opad eller nedad
                Der køber/sælger den lige efter toppunktet.
                Ved køb/salg går den ud af trenden (flag = 0) hvor den køber lavt, sælger højt, indtil den går udenfor en margin på graense (her = 100)
                hvor den gør det modsatte, da den forventer en trend (flag = 1)
                */
                int third_last = marketData[numElements-3];  // Get the third last price
                int second_last = marketData[numElements-2]; // Get the second last price
                int very_last = marketData[numElements-1];  // Get the last price
                int granse = 100;
                bool middle = (lastBuy+graense > very_last && lastBuy-graense < very_last);

                
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
                        Buy();
                        flag = 1;
                    }
                    else if(lastBuy > very_last)
                    {
                        Sell();
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
