using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BoilerPlate.App.API.Extensions;

/// <summary>
/// MVC settings
/// </summary>
public static class MvcExtensions
{
    /// <summary> Add controllers </summary>
    public static void AddControllersWithNormalizedEndpoints(this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
    }
}