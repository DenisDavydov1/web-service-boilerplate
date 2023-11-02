using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace BoilerPlate.Tests.Integration;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.Test.json", optional: false);
        });

        // builder.ConfigureServices(services =>
        // {
            // var dbContextDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbContextOptions<GervigDbContext>));
            //
            // services.Remove(dbContextDescriptor);
            //
            // var dbConnectionDescriptor = services.SingleOrDefault(
            //     d => d.ServiceType ==
            //          typeof(DbConnection));
            //
            // services.Remove(dbConnectionDescriptor);

            // services.AddDbContext<GervigDbContext>(options =>
            // {
            //     var configuration = new ConfigurationBuilder()
            //         .AddJsonFile("appsettings.IntTests.json")
            //         .Build();
            //
            //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            // });

            // var serviceProvider = services.BuildServiceProvider();
            //
            // using var serviceScope = serviceProvider.CreateScope();
            // var context = serviceScope.ServiceProvider.GetRequiredService<GervigDbContext>();
            // context.Database.EnsureDeleted();
            //
            // IntegrationTestsDatabaseInitializer.InitializeGervigDbContext(context);
        // });
    }
}