using System;
using System.Net.Http;

namespace GroupOne
{
    public static class Stonks
    {
        private static HttpClient _http = new HttpClient();

        public static int[] GetMarketData(DateTime from, DateTime to)
        {
            // Call the ruslan API and get the data 
            return new [] { 1 };
        }

        public static GroupInfo GetInfo()
        {
            // Call the ruslan API and get the info 
            return new GroupInfo();
        }

        public static void Buy(int quantity)
        {
            // Call the ruslan API and post 
            return;
        }

        public static void Sell(int quantity)
        {
            // Call the ruslan API and post
            return;
        }
    }

    public class GroupInfo
    {
        public string Name { get; set; }
        public int Balance { get; set; }
        public int StockCount { get; set; }
        public int StockValue { get; set; }
        public int TotalValue { get; set; }
    }
}