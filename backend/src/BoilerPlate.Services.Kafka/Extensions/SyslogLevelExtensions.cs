using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.Services.Kafka.Extensions;

public static class SyslogLevelExtensions
{
    public static LogLevel ToLogLevel(this SyslogLevel level) => level switch
    {
        SyslogLevel.Emergency or SyslogLevel.Alert or SyslogLevel.Critical => LogLevel.Critical,
        SyslogLevel.Error => LogLevel.Error,
        SyslogLevel.Warning => LogLevel.Warning,
        SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
        SyslogLevel.Debug => LogLevel.Debug,
        _ => throw new ArgumentOutOfRangeException()
    };
}