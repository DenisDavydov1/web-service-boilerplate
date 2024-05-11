using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Data.DAL.Extensions;

public static class DalServiceProviderExtensions
{
    public static async Task MigrateDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BoilerPlateDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}