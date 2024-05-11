using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.UnitOfWork;
using Npgsql;

namespace BoilerPlate.Data.DAL.Extensions;

public static class DalServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (EnvUtils.IsSwaggerGen || EnvUtils.IsDbInMemoryMode)
        {
            services.AddDbContext<BoilerPlateDbContext>(options => options.UseInMemoryDatabase("boilerPlate_db"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Connection string is missing.");
            }

            services.AddDbContext<BoilerPlateDbContext>(options =>
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
                dataSourceBuilder.EnableDynamicJson();
                options.UseNpgsql(dataSourceBuilder.Build());
            });
        }
    }

    public static void AddUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
}