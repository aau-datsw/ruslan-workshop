using System;
using System.Threading;

namespace Thiccpad
{
    class Program
    {
        static StonksUtils _stonks = new StonksUtils(false);

        private const double changeModeDiff = 0.001;
        private const double moveRefPointDiff = 0.008;

        private int changeModeLim {get; set;}
        private int moveRefPointLim {get; set;}
        private bool active {get; set;}

        static void Main(string[] args)
        {
          Program prog = new Program();
          GroupInfo info = _stonks.GetInfo();
          int[] marketData = GetMarketData();
          int stockValue = marketData[marketData.Length -1];
          prog.active = false;
          prog.updateLims(stockValue, prog.active);
          while (true) {
            marketData = GetMarketData();
            stockValue = marketData[marketData.Length -1];
            prog.MakeDecision(stockValue, prog.active);
          }
        }

        void MakeDecision(int stockValue, bool _active) 
        {
          if(_active) {
            if (stockValue < changeModeLim) {
              active = false;
              Sell();
              updateLims(stockValue, false);
            }
            if (stockValue > moveRefPointLim) {
              updateLims(stockValue, _active);
            }
          }
          else {
            if (stockValue > changeModeLim) {
              active = true;
              Buy();
              updateLims(stockValue, true);
            }
            if (stockValue < moveRefPointLim) {
              updateLims(stockValue, _active);
            }
          }
        }

        void updateLims(int refValue, bool _active)
        {
          moveRefPointLim = refValue;
          if (_active) {
            changeModeLim = (int) (refValue - refValue * changeModeDiff);
          }
          else {
            changeModeLim = (int) (refValue + refValue * changeModeDiff);
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
