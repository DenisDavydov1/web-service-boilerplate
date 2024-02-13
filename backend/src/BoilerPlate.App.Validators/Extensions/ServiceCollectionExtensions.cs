using BoilerPlate.Core.Utils;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.App.Validators.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(AssemblyUtils.BoilerPlateAssemblies);
    }
}