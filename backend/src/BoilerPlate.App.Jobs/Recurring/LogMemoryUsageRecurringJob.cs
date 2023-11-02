using System.Diagnostics;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.App.Jobs.Recurring;

public class LogMemoryUsageRecurringJob : IInvocable
{
    private readonly ILogger<LogMemoryUsageRecurringJob> _logger;

    public LogMemoryUsageRecurringJob(ILogger<LogMemoryUsageRecurringJob> logger) => _logger = logger;

    public Task Invoke()
    {
        var threadMemory = GC.GetAllocatedBytesForCurrentThread();
        var threadMemoryMb = threadMemory / 1024 / 1024;

        var currentProcess = Process.GetCurrentProcess();
        var processMemory = currentProcess.WorkingSet64;
        var processMemoryMb = processMemory / 1024 / 1024;

        _logger.LogDebug("Allocated memory: thread {ThreadMemory} MB, process {ProcessMemory} MB",
            threadMemoryMb, processMemoryMb);

        return Task.CompletedTask;
    }
}