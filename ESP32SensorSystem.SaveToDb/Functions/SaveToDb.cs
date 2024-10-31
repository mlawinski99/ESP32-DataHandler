using ESP32SensorSystem.SaveToDb.Helpers;
using ESP32SensorSystem.SaveToDb.Models;
using ESP32SensorSystem.SaveToDb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ESP32SensorSystem.SaveToDb.Functions;

public class SaveToDb(ILogger<SaveToDb> logger, ICosmosDbService cosmosDbService)
{
    [Function("SaveToDb")]
    public async Task<IActionResult> Run([HttpTrigger(
        AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonHelper.Deserialize<SensorDataModel>(requestBody);

            if (data.HasDefaultValues())
            {
                return new BadRequestObjectResult("Invalid request body");
            }

            await cosmosDbService.SaveAsync(data.ToSensorMeasurement());
            logger.LogInformation($"Data measured at {data.Time} successfully saved to Cosmos DB");
            
            return new OkObjectResult("Data saved successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }
}