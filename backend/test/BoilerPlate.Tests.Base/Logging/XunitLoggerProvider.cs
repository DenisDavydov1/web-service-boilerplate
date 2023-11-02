using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Base.Logging;

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IDictionary<string, LogLevel> _logLevels;

    public XunitLoggerProvider(ITestOutputHelper testOutputHelper, IConfiguration configuration)
    {
        _testOutputHelper = testOutputHelper;
        _logLevels = configuration.GetSection("Logging:LogLevel").Get<IDictionary<string, LogLevel>>()!;
    }

    public ILogger CreateLogger(string categoryName)
    {
        var isDefaultPresented = _logLevels.ContainsKey("Default");
        var minLogLevel = isDefaultPresented ? _logLevels["Default"] : default;

        var isCategoryPresented = _logLevels.Keys.Any(categoryName.StartsWith);
        if (isCategoryPresented)
        {
            minLogLevel = _logLevels
                .Last(x => categoryName.StartsWith(x.Key, StringComparison.InvariantCultureIgnoreCase))
                .Value;
        }

        return new XunitLogger(_testOutputHelper, categoryName, minLogLevel);
    }

    public void Dispose()
    {
    }
}