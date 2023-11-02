using Microsoft.Extensions.Hosting;

namespace BoilerPlate.Core.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsLocal(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.IsEnvironment("Local");

    public static bool IsTest(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.IsEnvironment("Test");
}