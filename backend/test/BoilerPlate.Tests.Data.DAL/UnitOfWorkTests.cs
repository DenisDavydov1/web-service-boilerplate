using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Tests.Base;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Data.DAL;

public class UnitOfWorkTests(ITestOutputHelper testOutputHelper) : BaseDbTests(testOutputHelper)
{
    [Fact]
    public void Repository_GetEqualRepositories_ShouldReturnSameObjects()
    {
        var unitOfWork = ServiceProvider.GetRequiredService<IUnitOfWork>();
        var usersRepository1 = unitOfWork.Repository<User>();
        var usersRepository2 = unitOfWork.Repository<User>();
        var usersRepo1Hash = usersRepository1.GetHashCode();
        var usersRepo2Hash = usersRepository2.GetHashCode();

        usersRepo1Hash.ShouldBe(usersRepo2Hash);
    }
}