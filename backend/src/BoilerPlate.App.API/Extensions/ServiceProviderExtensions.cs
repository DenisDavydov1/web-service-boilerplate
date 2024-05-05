namespace BoilerPlate.App.API.Extensions;

/// <summary> Service provider extensions </summary>
public static class ServiceProviderExtensions
{
    /// <summary> Service provider settings </summary>
    public static void ConfigureServiceProvider(this WebApplicationBuilder builder)
    {
        var serviceProviderOptions = builder.Configuration
            .GetSection(nameof(ServiceProviderOptions))
            .Get<ServiceProviderOptions>();

        builder.Host.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = serviceProviderOptions?.ValidateScopes ?? true;
            options.ValidateOnBuild = serviceProviderOptions?.ValidateOnBuild ?? false;
        });
    }
}