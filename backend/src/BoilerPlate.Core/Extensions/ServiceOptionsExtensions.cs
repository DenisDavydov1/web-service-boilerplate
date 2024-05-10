using BoilerPlate.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Core.Extensions;

public static class ServiceOptionsExtensions
{
    public static TServiceOptions AddServiceOptions<TServiceOptions>(
        this IServiceCollection services, IConfiguration configuration)
        where TServiceOptions : class, IServiceOptions
    {
        var optionsSection = configuration.GetSection(TServiceOptions.SectionName);
        services.Configure<TServiceOptions>(optionsSection);

        var options = optionsSection.Get<TServiceOptions>();
        if (options == null)
        {
            throw new Exception($"Configuration options {TServiceOptions.SectionName} are missing");
        }

        return options;
    }

    public static bool IsServiceEnabled<TConfigurationOptions>(this IConfiguration configuration)
        where TConfigurationOptions : class, IServiceOptions
    {
        var section = configuration.GetSection(TConfigurationOptions.SectionName);
        return section.Exists() && section.Get<TConfigurationOptions>()?.Enabled == true;
    }
}