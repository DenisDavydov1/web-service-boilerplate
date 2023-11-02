using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Services.System.Extensions;
using BoilerPlate.Tests.Base;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Services.Common;

public class BaseCommonServicesTests : BaseTests
{
    public BaseCommonServicesTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    protected override void AddServices(IServiceCollection services)
    {
        base.AddServices(services);

        services.AddSystemServices();
    }
}