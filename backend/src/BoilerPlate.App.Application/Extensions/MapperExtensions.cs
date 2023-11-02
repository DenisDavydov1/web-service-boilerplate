using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Core.Utils;

namespace BoilerPlate.App.Application.Extensions;

public static class MapperExtensions
{
    public static void AddMapper(this IServiceCollection services) =>
        services.AddAutoMapper(AssemblyUtils.BoilerPlateAssemblies);
}