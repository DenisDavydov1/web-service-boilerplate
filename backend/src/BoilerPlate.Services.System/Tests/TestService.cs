using Microsoft.Extensions.Logging;

namespace BoilerPlate.Services.System.Tests;

internal class TestService(ILogger<TestService> logger) : ITestService
{
    public void LogSomething() => logger.LogInformation("Logging from test service");
}