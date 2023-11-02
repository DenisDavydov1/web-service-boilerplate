using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Services.System.Tests;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Services.Common.Test;

public class TestServiceTests : BaseCommonServicesTests
{
    private readonly ITestService _testService;

    public TestServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) =>
        _testService = ServiceProvider.GetRequiredService<ITestService>();

    [Fact]
    public void LogSomething_UseXunitTestOutput_ShouldLog() =>
        _testService.LogSomething();
}