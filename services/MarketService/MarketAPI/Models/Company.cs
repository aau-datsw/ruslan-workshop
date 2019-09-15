using Newtonsoft.Json;

namespace MarketAPI.Models
{
  public partial class Company
  {
    [JsonProperty("id")]
    public int Id { get; set; } 
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("price")]
    public string Price { get; set; }
    [JsonProperty("volatility")]
    public int Volatility { get; set; }
  }
}