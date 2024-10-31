using System.Globalization;
using ESP32SensorSystem.SaveToDb.Entities;
using Microsoft.Azure.Cosmos;

namespace ESP32SensorSystem.SaveToDb.Services;

public sealed class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(string endpointUri, string primaryKey,
        string databaseId, string containerId)
    {
        var cosmosClient = new CosmosClient(endpointUri, primaryKey);
        var database = cosmosClient.GetDatabase(databaseId);
        _container = database.GetContainer(containerId);
    }

    public async Task SaveAsync(SensorMeasurement sensorMeasurement)
    {
        await _container.CreateItemAsync(
            sensorMeasurement, 
            new PartitionKey(sensorMeasurement.MeasurementTime.ToString(CultureInfo.InvariantCulture))
            );
    }
}