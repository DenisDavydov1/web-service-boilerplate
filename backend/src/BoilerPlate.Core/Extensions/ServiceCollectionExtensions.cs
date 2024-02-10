using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BoilerPlate.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSingletonAsServiceAsImplementation<TService, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService
        where TService : class
    {
        services.AddSingleton<TImplementation>();
        services.AddSingleton<TService>(provider => provider.GetRequiredService<TImplementation>());
    }

    public static void AddHostedServiceAsSingleton<THostedService>(this IServiceCollection services)
        where THostedService : class, IHostedService
    {
        services.AddSingleton<THostedService>();
        services.AddHostedService<THostedService>(provider => provider.GetRequiredService<THostedService>());
    }

    public static void SetDateTimeFormat(this IServiceCollection _)
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
        culture.DateTimeFormat.LongTimePattern = "";
        Thread.CurrentThread.CurrentCulture = culture;
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services) =>
        (IConfiguration) services.Single(x => x.ServiceType == typeof(IConfiguration)).ImplementationInstance!;
}