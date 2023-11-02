using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Base.Logging;

public class XunitLogger : ILogger
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly string _categoryName;
    private readonly LogLevel _minLogLevel;

    public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName, LogLevel minLogLevel)
    {
        _testOutputHelper = testOutputHelper;
        _categoryName = categoryName;
        _minLogLevel = minLogLevel;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => NoopDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel)
        => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (logLevel < _minLogLevel)
        {
            return;
        }

        _testOutputHelper.WriteLine($"{DateTime.UtcNow:u} {_categoryName} [{eventId}]:\n" +
                                    $"{formatter(state, exception)}");
        if (exception != null)
            _testOutputHelper.WriteLine(exception.ToString());
    }

    private class NoopDisposable : IDisposable
    {
        public static readonly NoopDisposable Instance = new();

        public void Dispose()
        {
        }
    }
}