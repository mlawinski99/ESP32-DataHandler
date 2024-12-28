using ESP32SensorSystem.SaveToDb.Entities;

namespace ESP32SensorSystem.SaveToDb.Services;

public interface ICosmosDbService
{
    Task SaveAsync(SensorDataInternalModel dataInternalModel);
}