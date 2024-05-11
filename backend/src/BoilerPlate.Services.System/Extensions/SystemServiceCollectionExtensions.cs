using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Services.System.Tests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.Services.System.Extensions;

public static class SystemServiceCollectionExtensions
{
    public static void AddSystemServices(this IServiceCollection services)
    {
        services.AddTransient<ITestService, TestService>();
        services.AddScoped<IUsersService, UsersService>();
    }
}