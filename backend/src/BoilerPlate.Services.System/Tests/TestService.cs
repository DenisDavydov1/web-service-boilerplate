using Microsoft.Extensions.Logging;

namespace BoilerPlate.Services.System.Tests;

internal class TestService : ITestService
{
    private readonly ILogger<TestService> _logger;

    public TestService(ILogger<TestService> logger) => _logger = logger;

    public void LogSomething() => _logger.LogInformation("Logging from test service");
}