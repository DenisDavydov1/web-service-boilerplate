using BoilerPlate.App.API;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
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
        var (user, _) = await UserSeedFactory.SeedAsync(UnitOfWork);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        var response = await GetAsync($"api/users/{user.Id}");
        var userDto = await response.ToDtoAsync<UserDto>();

        response.EnsureSuccessStatusCode();
        AssertUserDto(userDto, user);
    }

    [Fact]
    public async Task GetCurrentAsync_AuthorizedUser_ShouldSuccess()
    {
        var user = await UnitOfWork.Repository<User>().GetAsync(x => x.Login == SeedConstants.UserLogin);
        await AuthorizeAsync(SeedConstants.UserLogin, SeedConstants.UserPassword);

        var response = await GetAsync("api/users/current");
        var userDto = await response.ToDtoAsync<UserDto>();

        response.EnsureSuccessStatusCode();
        AssertUserDto(userDto, user!);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsFirstPage()
    {
        // Arrange
        await SeedUsers(11);
        var totalUsers = UnitOfWork.Repository<User>().GetAllAsQueryable().Count();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync("api/users?page=1&pageSize=5");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Items.Count().ShouldBe(5);
        getAllDto.Page.ShouldBe(1);
        getAllDto.PageSize.ShouldBe(5);
        getAllDto.TotalItems.ShouldBe(totalUsers);
        getAllDto.HasMore.ShouldBeTrue();
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsSecondPage()
    {
        // Arrange
        await SeedUsers(21);
        var totalUsers = UnitOfWork.Repository<User>().GetAllAsQueryable().Count();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?page=2&pageSize=5");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Items.Count().ShouldBe(5);
        getAllDto.Page.ShouldBe(2);
        getAllDto.PageSize.ShouldBe(5);
        getAllDto.TotalItems.ShouldBe(totalUsers);
        getAllDto.HasMore.ShouldBeTrue();
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsLastPage()
    {
        // Arrange
        await SeedUsers(11);
        var totalUsers = UnitOfWork.Repository<User>().GetAllAsQueryable().Count();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        const int pageSize = 5;
        var page = totalUsers % pageSize == 0
            ? totalUsers / pageSize
            : (totalUsers / pageSize) + 1;

        // Act
        var response = await GetAsync($"api/users?page={page}&pageSize={pageSize}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Items.Count().ShouldBe(totalUsers % pageSize == 0 ? pageSize : totalUsers % pageSize);
        getAllDto.Page.ShouldBe(page);
        getAllDto.PageSize.ShouldBe(pageSize);
        getAllDto.TotalItems.ShouldBe(totalUsers);
        getAllDto.HasMore.ShouldBeFalse();
    }

    private void AssertUserDto(UserDto userDto, User user)
    {
        userDto.Login.ShouldBe(user.Login);
        userDto.Name.ShouldBe(user.Name);
        userDto.Email.ShouldBe(user.Email);
        userDto.LanguageCode.ShouldBe(user.LanguageCode);
        userDto.Role.ShouldBe(user.Role);
    }

    private async Task SeedUsers(int count)
    {
        for (var i = 0; i < count; i++)
            await UserSeedFactory.SeedAsync(UnitOfWork);
    }
}