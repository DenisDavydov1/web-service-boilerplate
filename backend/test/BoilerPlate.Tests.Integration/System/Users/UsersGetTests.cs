using System.Net;
using BoilerPlate.App.API;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Users.Responses;
using BoilerPlate.Data.Seeds.Constants;
using BoilerPlate.Tests.Integration.Extensions;
using BoilerPlate.Tests.Seeds.SeedFactories.System.Users;
using Microsoft.Extensions.DependencyInjection;
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

    protected override void AddServices(IServiceCollection services) => base.AddServices(services);
    protected override void AddHostServices(IServiceCollection services) => base.AddHostServices(services);

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
        var response = await GetAsync("api/users?page=1&resultsPerPage=5");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBe(5);
        getAllDto.Page.ShouldBe(1);
        getAllDto.ResultsPerPage.ShouldBe(5);
        getAllDto.TotalResults.ShouldBe(totalUsers);
        getAllDto.IsLast.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsSecondPage()
    {
        // Arrange
        await SeedUsers(21);
        var totalUsers = UnitOfWork.Repository<User>().GetAllAsQueryable().Count();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?page=2&resultsPerPage=5");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBe(5);
        getAllDto.Page.ShouldBe(2);
        getAllDto.ResultsPerPage.ShouldBe(5);
        getAllDto.TotalResults.ShouldBe(totalUsers);
        getAllDto.IsLast.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsLastPage()
    {
        // Arrange
        await SeedUsers(11);
        var totalUsers = UnitOfWork.Repository<User>().GetAllAsQueryable().Count();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        const int resultsPerPage = 5;
        var page = totalUsers % resultsPerPage == 0
            ? totalUsers / resultsPerPage
            : (totalUsers / resultsPerPage) + 1;

        // Act
        var response = await GetAsync($"api/users?page={page}&resultsPerPage={resultsPerPage}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBe(totalUsers % resultsPerPage == 0 ? resultsPerPage : totalUsers % resultsPerPage);
        getAllDto.Page.ShouldBe(page);
        getAllDto.ResultsPerPage.ShouldBe(resultsPerPage);
        getAllDto.TotalResults.ShouldBe(totalUsers);
        getAllDto.IsLast.ShouldBeTrue();
    }

    [Fact]
    public async Task GetAllAsync_WithSort_SortsAsc()
    {
        // Arrange
        await SeedUsers(10);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        var allLoginsSortedAsc = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .OrderBy(x => x.Login)
            .Take(100)
            .Select(x => x.Login)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?sort=login:asc");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();
        var dtoLogins = getAllDto.Content.Select(x => x.Login).ToArray();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(10);
        dtoLogins.ShouldBe(allLoginsSortedAsc);
    }

    [Fact]
    public async Task GetAllAsync_WithSort_SortsDesc()
    {
        // Arrange
        await SeedUsers(10);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        var allLoginsSortedDesc = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .OrderByDescending(x => x.Login)
            .Take(100)
            .Select(x => x.Login)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?sort=login:desc");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();
        var dtoLogins = getAllDto.Content.Select(x => x.Login).ToArray();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(10);
        dtoLogins.ShouldBe(allLoginsSortedDesc);
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleSorts_Sorts()
    {
        // Arrange
        await SeedUsers(10);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        var allLoginsSorted = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .OrderBy(x => x.Login)
            .ThenByDescending(x => x.Id)
            .Take(100)
            .Select(x => x.Id)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?sort=login:asc,id:desc");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();
        var dtoLogins = getAllDto.Content.Select(x => x.Id).ToArray();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(10);
        dtoLogins.ShouldBe(allLoginsSorted);
    }

    [Theory]
    [InlineData("dogin:asc", null)]
    [InlineData("login:ascending", ExceptionCode.Common_GetAll_InvalidSortString)]
    [InlineData("login:a", ExceptionCode.Common_GetAll_InvalidSortString)]
    [InlineData("login", ExceptionCode.Common_GetAll_InvalidSortString)]
    public async Task GetAllAsync_SortByInvalidProperty_ReturnsError(string sort, ExceptionCode? expectedErrorCode)
    {
        // Arrange
        await SeedUsers(10);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?sort={sort}");
        var errorCode = await response.GetErrorCode();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorCode.ShouldBe(expectedErrorCode?.ToString());
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