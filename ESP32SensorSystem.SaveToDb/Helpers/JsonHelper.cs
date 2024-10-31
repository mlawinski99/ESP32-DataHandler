using Newtonsoft.Json;

namespace ESP32SensorSystem.SaveToDb.Helpers;

public static class JsonHelper
{
    public static T Deserialize<T>(string value)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch (JsonReaderException)
        {
            return default;
        }
    }
}