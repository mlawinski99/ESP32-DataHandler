namespace ESP32SensorSystem.SaveToDb.Models;

public class SensorDataInternalModel
{
    public float TemperatureInside { get; set; }
    public float TemperatureOutside { get; set; }
    public float HumidityInside { get; set; }
    public float HumidityOutside { get; set; }
    public float Pressure { get; set; }
    public string MeasurementTime { get; set; }
}