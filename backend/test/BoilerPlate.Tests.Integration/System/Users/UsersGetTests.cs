using BoilerPlate.App.API;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Users.Responses;
using BoilerPlate.Data.Seeds.Constants;
using BoilerPlate.Tests.Integration.Extensions;
using BoilerPlate.Tests.Seeds.SeedFactories.System.Users;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Integration.System.Users;

public class UsersGetTests : BaseIntegrationTests
{
    public UsersGetTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task GetByIdAsync_AdminRole_ShouldSuccess()
    {
        var user = await UserSeedFactory.SeedAsync(UnitOfWork);
        await AuthorizeAsync(UserRole.Admin);

        var response = await GetAsync($"api/users/{user.Id}");
        var userDto = await response.ToDtoAsync<UserDto>();

        response.EnsureSuccessStatusCode();
        AssertUserDto(userDto, user);
    }

    [Fact]
    public async Task GetCurrentAsync_AuthorizedUser_ShouldSuccess()
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Login == SeedConstants.UserLogin);
        await AuthorizeAsync(UserRole.User);

        var response = await GetAsync("api/users/current");
        var userDto = await response.ToDtoAsync<UserDto>();

        response.EnsureSuccessStatusCode();
        AssertUserDto(userDto, user!);
    }

    private void AssertUserDto(UserDto userDto, User user)
    {
        userDto.Login.ShouldBe(user.Login);
        userDto.Name.ShouldBe(user.Name);
        userDto.Email.ShouldBe(user.Email);
        userDto.LanguageCode.ShouldBe(user.LanguageCode);
        userDto.Role.ShouldBe(user.Role);
    }
}