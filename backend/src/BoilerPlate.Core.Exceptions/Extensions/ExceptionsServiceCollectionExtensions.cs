using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Core.Exceptions.Factory;

namespace BoilerPlate.Core.Exceptions.Extensions;

public static class ExceptionsServiceCollectionExtensions
{
    public static void AddExceptions(this IServiceCollection services)
        => services.AddScoped<IExceptionFactory, ExceptionFactory>();
}