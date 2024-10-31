using ESP32SensorSystem.SaveToDb.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<ICosmosDbService>(x => new CosmosDbService(
            Environment.GetEnvironmentVariable("CosmosDbEndpointUri"),
            Environment.GetEnvironmentVariable("CosmosDbPrimaryKey"),
            Environment.GetEnvironmentVariable("CosmosDbDatabaseId"),
            Environment.GetEnvironmentVariable("CosmosDbContainerId")));
    })
    .Build();

host.Run();