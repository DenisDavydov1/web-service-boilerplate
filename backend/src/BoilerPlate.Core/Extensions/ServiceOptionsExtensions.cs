using BoilerPlate.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Core.Extensions;

public static class ServiceOptionsExtensions
{
    public static void AddServiceOptions<TServiceOptions>(
        this IServiceCollection services, TServiceOptions options)
        where TServiceOptions : class, IServiceOptions, new() =>
        services.AddSingleton(_ => Microsoft.Extensions.Options.Options.Create(options));

    public static TServiceOptions AddServiceOptions<TServiceOptions>(
        this IServiceCollection services, IConfiguration configuration,
        (string property, (string? typeName, Type type)[])[]? polymorphicArraysDescription = null)
        where TServiceOptions : class, IServiceOptions, new()
    {
        var options = configuration.GetServiceOptions<TServiceOptions>(polymorphicArraysDescription);
        services.AddServiceOptions(options);

        return options;
    }

    public static TServiceOptions GetServiceOptions<TServiceOptions>(this IConfiguration configuration,
        (string property, (string? typeName, Type type)[])[]? polymorphicArraysDescription = null)
        where TServiceOptions : class, IServiceOptions, new()
    {
        var options = new TServiceOptions();
        configuration.GetSection(TServiceOptions.SectionName).Bind(options);

        if (polymorphicArraysDescription?.Length > 0)
        {
            foreach (var (arrayPropertyName, elementPropertiesTypes) in polymorphicArraysDescription)
            {
                var elementsOptions = new List<BasePolymorphicArrayElementOptions>();

                for (var i = 0;; i++)
                {
                    var root = $"{TServiceOptions.SectionName}:{arrayPropertyName}:{i}";
                    if (configuration.GetSection(root).Exists() == false)
                    {
                        break;
                    }

                    var typeName = configuration.GetValue<string>($"{root}:Type");
                    var type = elementPropertiesTypes.First(x => x.typeName == typeName).type;

                    if (Activator.CreateInstance(type) is not BasePolymorphicArrayElementOptions elementOptions)
                    {
                        throw new Exception($"Invalid {typeName} type options format");
                    }

                    configuration.GetSection(root).Bind(elementOptions);
                    elementsOptions.Add(elementOptions);
                }

                var arrayProperty = typeof(TServiceOptions).GetProperty(arrayPropertyName);
                if (arrayProperty == null)
                {
                    throw new Exception(
                        $"Property {arrayPropertyName} is not found in {TServiceOptions.SectionName} options");
                }

                arrayProperty.SetValue(options, elementsOptions);
            }
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