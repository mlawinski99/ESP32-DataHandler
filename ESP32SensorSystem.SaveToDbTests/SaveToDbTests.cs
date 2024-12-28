using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using ESP32SensorSystem.SaveToDb.Entities;
using ESP32SensorSystem.SaveToDb.Functions;
using ESP32SensorSystem.SaveToDb.Models;
using ESP32SensorSystem.SaveToDb.Services;
using Microsoft.AspNetCore.Mvc;

public class SaveToDbTests
{
    private readonly Mock<ILogger<SaveToDb>> _mockLogger;
    private readonly Mock<ICosmosDbService> _mockCosmosDbService;
    private readonly SaveToDb _function;

    public SaveToDbTests()
    {
        _mockLogger = new Mock<ILogger<SaveToDb>>();
        _mockCosmosDbService = new Mock<ICosmosDbService>();
        _function = new SaveToDb(_mockLogger.Object, _mockCosmosDbService.Object);
    }

    [Fact]
    public async Task Run_ReturnsOkResult_WhenDataIsValid()
    {
        // Arrange
        var sensorData = new SensorDataExternalModel
        {
            Time = "2024-10-01T12:00:00Z",
            TempIn = 20.0f,
            TempOut = 19.0f,
            HumidityIn = 50.0f,
            HumidityOut = 49.0f,
            Pressure = 1013.25f
        };
        var requestBody = JsonConvert.SerializeObject(sensorData);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

        var request = httpContext.Request;

        _mockCosmosDbService.Setup(x => 
                x.SaveAsync(It.IsAny<SensorDataInternalModel>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _function.Run(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Data saved successfully", okResult.Value);
        _mockCosmosDbService.Verify(x => x.SaveAsync(It.IsAny<SensorDataInternalModel>()), Times.Once);
    }
    
    [Fact]
    public async Task Run_ReturnsBadRequest_WhenRequestBodyIsInvalid()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{\"invalidData\": \"123\"}"));

        var request = httpContext.Request;

        // Act
        var result = await _function.Run(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid request body", badRequestResult.Value);
    }

    [Fact]
    public async Task Run_LogsError_WhenExceptionIsThrown()
    {
        // Arrange
        var sensorData = new SensorDataExternalModel 
        {
            Time = "2024-10-01T12:00:00Z",
            TempIn = 20.0f,
            TempOut = 19.0f,
            HumidityIn = 50.0f,
            HumidityOut = 49.0f,
            Pressure = 1013.25f
        };
            
        var requestBody = JsonConvert.SerializeObject(sensorData);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

        var request = httpContext.Request;

        var exceptionMessage = "Error saving to DB";
        _mockCosmosDbService.Setup(x => 
                x.SaveAsync(It.IsAny<SensorDataInternalModel>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _function.Run(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(exceptionMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
        
        Assert.IsType<BadRequestResult>(result);
    }
}