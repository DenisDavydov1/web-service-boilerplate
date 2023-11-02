using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Core.Utils;

namespace BoilerPlate.App.Application.Extensions;

public static class ValidatorExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(AssemblyUtils.BoilerPlateAssemblies);
    }
}