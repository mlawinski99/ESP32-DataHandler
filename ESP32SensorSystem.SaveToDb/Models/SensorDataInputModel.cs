using ESP32SensorSystem.SaveToDb.Entities;

namespace ESP32SensorSystem.SaveToDb.Models;

public class SensorDataExternalModel
{
    public float TempIn { get; set; }
    public float TempOut { get; set; }
    public float HumidityIn { get; set; }
    public float HumidityOut { get; set; }
    public float Pressure { get; set; }
    public string Time { get; set; }

    public bool HasDefaultValues()
    {
        return TempIn == default &&
               TempOut == default &&
               HumidityIn == default &&
               HumidityOut == default &&
               Pressure == default &&
               Time == default;
    }
    
    public SensorDataInternalModel ToSensorMeasurement()
    {
        return new SensorDataInternalModel
        {
            TemperatureInside = TempIn,
            TemperatureOutside = TempOut,
            HumidityInside = HumidityIn,
            HumidityOutside = HumidityOut,
            Pressure = Pressure,
            MeasurementTime = DateTime.Parse(Time).ToString("o")
        };
    }
}