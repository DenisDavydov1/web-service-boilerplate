using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Data.DAL.Extensions;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Seeds.Extensions;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Base;

public abstract class BaseDbTests : BaseTests, IDisposable
{
    protected IUnitOfWork UnitOfWork;

    public BaseDbTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) =>
        UnitOfWork = ServiceProvider.GetRequiredService<IUnitOfWork>();

    public virtual void Dispose() => UnitOfWork.Dispose();

    protected override void AddServices(IServiceCollection services)
    {
        base.AddServices(services);

        var configuration = (IConfiguration) services.Single(x => x.ServiceType == typeof(IConfiguration))
            .ImplementationInstance!;
        services.AddDatabase(configuration);
        services.AddUnitOfWork();
    }

    protected override void UseServices(IServiceProvider services)
    {
        base.UseServices(services);

        services.MigrateDatabase().GetAwaiter().GetResult();
        services.SeedDatabase().GetAwaiter().GetResult();
    }
}