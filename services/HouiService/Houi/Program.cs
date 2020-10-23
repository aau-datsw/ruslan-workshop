using System;
using System.Threading;

namespace Houi
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

                // stig == køb
                // fald == salg
                Større_end_eller_mindre_end(marketData, numElements);
            }
        }


        static void Større_end_eller_mindre_end(int[] marketData, int numElements){
            if (marketData[numElements - 11] < marketData[numElements - 1]){
                Buy();
            }

            if (marketData[numElements - 11] > marketData[numElements - 1]){
                Sell();
            }
        }











        static void Avrage_over_4_dele(int[] marketData, int numElements)
        {
            int avrage_of_all = 0, first_half_avrage = 0, second_half_avrage = 0, third_half_avrage = 0, fourth_half_avrage = 0;

            //calculates avrage of first half
            for (int a = 0; a < (numElements - 1) * 1 / 4; a++)
            {
                first_half_avrage += marketData[a];
            }
            first_half_avrage = first_half_avrage / (numElements - 1) * 1 / 4;


            //calculates avrage of second half
            for (int a = (numElements - 1) * 1 / 4; a < (numElements - 1) * 2 / 4; a++)
            {
                second_half_avrage += marketData[a];
            }
            second_half_avrage = second_half_avrage / (numElements - 1) * 2 / 4;


            //calculates avrage of second half
            for (int a = (numElements - 1) * 2 / 4; a < (numElements - 1) * 3 / 4; a++)
            {
                third_half_avrage += marketData[a];
            }
            third_half_avrage = third_half_avrage / (numElements - 1) * 3 / 4;


            //calculates avrage of second half
            for (int a = (numElements - 1) * 3 / 4; a < (numElements - 1); a++)
            {
                fourth_half_avrage += marketData[a];
            }
            fourth_half_avrage = fourth_half_avrage / (numElements - 1);


            //calculates avrage of all the data
            for (int a = 0; a < numElements - 1; a++)
            {
                avrage_of_all += marketData[a];
            }
            avrage_of_all = avrage_of_all / numElements;


            if (first_half_avrage < second_half_avrage && second_half_avrage < third_half_avrage && third_half_avrage > fourth_half_avrage && marketData[numElements - 1] > avrage_of_all)
            {
                Sell();
            }

            if (first_half_avrage > second_half_avrage && second_half_avrage > third_half_avrage && third_half_avrage < fourth_half_avrage && marketData[numElements - 1] > avrage_of_all)
            {
                Buy();
            }
        }



        static void tutorens(int[] marketData, int numElements)
        {
            int firstPrice = marketData[0];  // Get the first price 
            int lastPrice = marketData[numElements - 1];  // Get the last price

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
            }
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
