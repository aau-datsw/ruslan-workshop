using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace JansKreditfond
{

    public class StonksUtils
    {
        private HttpClient _http;
        private string _port; 
        private string _host;
        private string _grpName = "JansKreditfond";

        public StonksUtils()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("X-Token", "1f374e33-c5ec-454e-8ffa-efd7bb368dd5");
            _port = "";
            _host = "717-130-225-198-158.ngrok.io";
            Console.WriteLine($"Successfully started a Stonk Trader for {_grpName} using port {_port}...");
        }

        public int[] GetMarketData(DateTime from, DateTime to)
        {
            // Call the ruslan API and get the data 
            var response = _http.GetAsync($"http://{_host}/api/v1/market?from={ISO8601(from)}&to={ISO8601(to)}")
                .GetAwaiter()
                .GetResult();

            var data = JsonConvert.DeserializeObject<List<dynamic>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            
            return data.Select(o => (int) o.price).ToArray();
        }

        public GroupInfo GetInfo()
        {
            // Call the ruslan API and get the info 
            var response = _http.GetAsync($"http://{_host}/api/v1/account").GetAwaiter().GetResult();
            var rawJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var info = JsonConvert.DeserializeObject<GroupInfo>(rawJson);
            System.Console.WriteLine(info);
            return info;
        }

        public void Buy()
        {
            _http.PostAsync($"http://{_host}/api/v1/buy", new StringContent("")).GetAwaiter().GetResult();
        }

        public void Sell()
        {
            _http.PostAsync($"http://{_host}/api/v1/sell", new StringContent("")).GetAwaiter().GetResult();
        }

        private static string ISO8601(DateTime date)
        {
            return date.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class GroupInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("balance")]
        public int Balance { get; set; }
        [JsonProperty("stonk_count")]
        public int StockCount { get; set; }
        [JsonProperty("stonk_value")]
        public int StockValue { get; set; }
        [JsonProperty("total_value")]
        public int TotalValue { get; set; }

        public override string ToString() => $"{Name} ({Balance} {StockCount} {StockValue} {TotalValue})";
    }
}