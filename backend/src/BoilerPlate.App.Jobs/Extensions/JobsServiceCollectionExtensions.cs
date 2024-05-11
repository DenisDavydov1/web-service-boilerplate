using Coravel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BoilerPlate.App.Jobs.Fire;
using BoilerPlate.App.Jobs.Recurring;

namespace BoilerPlate.App.Jobs.Extensions;

public static class JobsServiceCollectionExtensions
{
    public static void AddJobs(this IServiceCollection services)
    {
        // Hosted services
        // services.AddHostedServiceAsSingleton<TestTimedHostedService>();

        // Jobs
        services.AddQueue();
        services.AddScheduler();
        services.AddTransient<LogFireJob>();
        services.AddTransient<LogMemoryUsageRecurringJob>();
    }

    public static void UseJobs(this IServiceProvider services) =>
        services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<LogMemoryUsageRecurringJob>().EveryMinute(); // .Cron("* * * * *");
        });
}