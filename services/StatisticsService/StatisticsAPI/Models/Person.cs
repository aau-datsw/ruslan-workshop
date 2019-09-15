using Newtonsoft.Json;

namespace StatisticsAPI.Models
{
  public partial class Person
  {
    [JsonProperty("id")]
    public int Id { get; set; } 
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("other_name")]
    public string OtherName { get; set; }
  }
}