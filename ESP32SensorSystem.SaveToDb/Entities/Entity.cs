using Newtonsoft.Json;

namespace ESP32SensorSystem.SaveToDb.Entities;

public record Entity<T>
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [JsonProperty("body")]
    public T Body { get; set; }
    
    [JsonProperty("MeasurementTime")]
    public string MeasurementTime { get; set; }
}