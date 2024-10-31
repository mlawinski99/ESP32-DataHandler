namespace ESP32SensorSystem.SaveToDb.Entities;

public class SensorMeasurement : Entity
{
    public float TemperatureInside { get; set; }
    public float TemperatureOutside { get; set; }
    public float HumidityInside { get; set; }
    public float HumidityOutside { get; set; }
    public float Pressure { get; set; }
    public DateTime MeasurementTime { get; set; }
}