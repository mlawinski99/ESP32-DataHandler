using ESP32SensorSystem.SaveToDb.Entities;
using ESP32SensorSystem.SaveToDb.Models;

namespace ESP32SensorSystem.SaveToDb.Services;

public interface ICosmosDbService
{
    Task SaveAsync(SensorDataInternalModel dataInternalModel);
}