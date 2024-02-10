using BoilerPlate.App.API;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.Seeds.Constants;
using BoilerPlate.Tests.Integration.Extensions;
using BoilerPlate.Tests.Seeds.Fakers.DTO.System.Users;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Integration.System.Users;

public class UsersPostTests : BaseIntegrationTests
{
    public UsersPostTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task CreateUserAsync_AnonymousRegister_ShouldCreateUserRole()
    {
        var request = new RegisterUserDtoFaker().Generate();

        var response = await PostAsync("api/users/register", request);
        var createdDto = await response.ToDtoAsync<IdDto>();
        var user = await UnitOfWork.Repository<User>().GetByIdAsync(createdDto.Id);

        response.EnsureSuccessStatusCode();
        user.ShouldNotBeNull();
        user.Login.ShouldBe(request.Login);
        HashingUtils.VerifyBCrypt(request.Password, user.PasswordHash).ShouldBeTrue();
        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
        user.LanguageCode.ShouldBe(request.LanguageCode);
        user.Role.ShouldBe(UserRole.User);
        user.CreatedBy.ShouldBe(SeedConstants.RootUserId);
    }
}