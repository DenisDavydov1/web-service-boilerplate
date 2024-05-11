using BoilerPlate.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.App.Mappers.Extensions;

public static class MappersServiceCollectionExtensions
{
    public static void AddMappers(this IServiceCollection services) =>
        services.AddAutoMapper(AssemblyUtils.BoilerPlateAssemblies);
}