using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public int Price { get; set; }
    [JsonProperty("volatility")]
    public int Volatility { get; set; }

    public ICollection<Record> History { get; set; }

    public override string ToString() => $"{Id}: {Name}@{Price}DKK ({(Volatility == 0 ? "SLOW" : Volatility == 1 ? "NORMAL" : "VOLATILE")})";
  }

  public partial class Record
  {
    public int Id { get; set; }
    public int Timestamp { get; set; }
    public int Price { get; set; }
  }
}