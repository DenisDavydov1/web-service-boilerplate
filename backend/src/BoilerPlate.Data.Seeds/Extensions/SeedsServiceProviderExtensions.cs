using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Seeds.Common;

namespace BoilerPlate.Data.Seeds.Extensions;

public static class SeedsServiceProviderExtensions
{
    public static async Task SeedDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var seedsTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && typeof(BaseSeeds).IsAssignableFrom(t));
        var allSeeds = seedsTypes.Select(Activator.CreateInstance).ToArray();
        foreach (BaseSeeds? seeds in allSeeds)
        {
            if (seeds != null) await seeds.SeedAsync(unitOfWork);
        }
    }
}