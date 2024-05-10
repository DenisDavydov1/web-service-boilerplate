using BoilerPlate.App.API;
using BoilerPlate.Data.Abstractions.Enums;
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

public class UsersGetTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    : BaseIntegrationTests(factory, testOutputHelper)
{
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
        var allIdsSorted = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .OrderBy(x => x.Login)
            .ThenByDescending(x => x.Id)
            .Take(100)
            .Select(x => x.Id)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?sort=login:asc,id:desc");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();
        var dtoIds = getAllDto.Content.Select(x => x.Id).ToArray();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(10);
        dtoIds.ShouldBe(allIdsSorted);
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_FiltersBySubstring()
    {
        // Arrange
        await SeedUsers(10);
        if (await UnitOfWork.Repository<User>().ExistsAsync(x => x.Login == "www") == false)
        {
            await UserSeedFactory.SeedAsync(UnitOfWork, o => o.RuleFor = f => f.Login = "www");
        }

        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        var allLoginsFiltered = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .Where(x => x.Login.Contains("w"))
            .Select(x => x.Login)
            .Take(100)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?filter=login:w");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();
        var dtoLogins = getAllDto.Content.Select(x => x.Login).ToArray();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(1);
        dtoLogins.ShouldBe(allLoginsFiltered);
        dtoLogins.ShouldAllBe(x => x.Contains("w"));
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_FiltersByStringWithCommaAndColon()
    {
        // Arrange
        const string email = ",:one:two:three,|\\@#$%^&*()_-4,:,";
        await UserSeedFactory.SeedAsync(UnitOfWork, o => o.RuleFor = f => f.Email = email);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?filter=email:{email}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(1);
        getAllDto.Content.ShouldAllBe(x => x.Email == email);
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_FiltersByEnum()
    {
        // Arrange
        await SeedUsers(10);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);
        var allIdsFiltered = UnitOfWork.Repository<User>().GetAllAsQueryable()
            .Where(x => x.Role == UserRole.Admin)
            .Select(x => x.Id)
            .Take(100)
            .ToArray();

        // Act
        var response = await GetAsync("api/users?filter=role:Admin");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(1);
        getAllDto.Content.Select(x => x.Id).ShouldBe(allIdsFiltered);
        getAllDto.Content.ShouldAllBe(x => x.Role == UserRole.Admin);
    }

    [Theory]
    [InlineData("05/21/2023")]
    [InlineData("2023-05-21")]
    [InlineData("2023-05-21 00:00:00")]
    [InlineData("2023-05-21T00:00:00")]
    [InlineData("2023-05-20T21:00:00Z")]
    public async Task GetAllAsync_WithFilter_FiltersByDateTime(string dateFilter)
    {
        // Arrange
        var date = new DateTime(2023, 05, 21).ToUniversalTime();
        var user = await UserSeedFactory.SeedAsync(UnitOfWork, o => o.RuleFor = f => f.CreatedAt = date);
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?filter=createdAt:{dateFilter}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(1);
        getAllDto.Content.Any(x => x.Id == user.User.Id).ShouldBeTrue();
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_FiltersByGuid()
    {
        // Arrange
        var users = await SeedUsers(2);
        var user = users.First();
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?filter=id:{user.Id}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBe(1);
        getAllDto.Content.First().Id.ShouldBe(user.Id);
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_FiltersByMultipleFields()
    {
        // Arrange
        const string email = "one@mail.com";
        await UserSeedFactory.SeedAsync(UnitOfWork, o => o.RuleFor = f =>
        {
            f.Email = email;
            f.Role = UserRole.Moderator;
        });
        await UserSeedFactory.SeedAsync(UnitOfWork, o => o.RuleFor = f =>
        {
            f.Email = email;
            f.Role = UserRole.Admin;
        });
        await AuthorizeAsync(SeedConstants.AdminLogin, SeedConstants.AdminPassword);

        // Act
        var response = await GetAsync($"api/users?filter=email:{email},role:{UserRole.Moderator}");
        var getAllDto = await response.ToDtoAsync<GetAllDto<UserDto>>();

        // Assert
        getAllDto.Content.Count().ShouldBeGreaterThanOrEqualTo(1);
        getAllDto.Content.ShouldAllBe(x => x.Email == email && x.Role == UserRole.Moderator);
    }

    private void AssertUserDto(UserDto userDto, User user)
    {
        userDto.Login.ShouldBe(user.Login);
        userDto.Name.ShouldBe(user.Name);
        userDto.Email.ShouldBe(user.Email);
        userDto.LanguageCode.ShouldBe(user.LanguageCode);
        userDto.Role.ShouldBe(user.Role);
    }

    private async Task<IEnumerable<User>> SeedUsers(int count)
    {
        var users = new List<User>();

        for (var i = 0; i < count; i++)
        {
            var user = await UserSeedFactory.SeedAsync(UnitOfWork);
            users.Add(user.User);
        }

        return users;
    }
}