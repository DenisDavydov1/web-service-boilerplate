using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BoilerPlate.App.Application.Extensions;
using BoilerPlate.Tests.Base.Logging;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Base;

public abstract class BaseTests
{
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly IServiceProvider ServiceProvider;

    protected BaseTests(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
        ServiceProvider = InitializeServiceProvider();
    }

    protected virtual void AddServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", optional: false)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddLogging(builder =>
        {
            builder.AddProvider(new XunitLoggerProvider(TestOutputHelper, configuration));
        });

        services.AddMediator();
    }

    protected virtual void UseServices(IServiceProvider services)
    {
    }

    private ServiceProvider InitializeServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        AddServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        UseServices(serviceProvider);

        return serviceProvider;
    }

    protected async Task RunBenchmarkTest(Func<Task> test)
    {
        WriteMemoryStats();
        var sw = Stopwatch.StartNew();

        await test();

        sw.Stop();
        TestOutputHelper.WriteLine($"Elapsed time: {sw.Elapsed.ToString()}");

        WriteMemoryStats();
    }

    private void WriteMemoryStats()
    {
        var threadMemory = GC.GetAllocatedBytesForCurrentThread();
        TestOutputHelper.WriteLine($"Allocated memory thread: {threadMemory / 1024 / 1024} MB");

        var currentProcess = Process.GetCurrentProcess();
        var processMemory = currentProcess.WorkingSet64;
        TestOutputHelper.WriteLine($"Allocated memory process: {processMemory / 1024 / 1024} MB");
    }
}