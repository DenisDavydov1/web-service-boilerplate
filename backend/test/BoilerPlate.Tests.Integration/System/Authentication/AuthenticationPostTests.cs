using BoilerPlate.App.API;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;
using BoilerPlate.Data.Seeds.Constants;
using BoilerPlate.Tests.Integration.Extensions;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Integration.System.Authentication;

public class AuthenticationPostTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    : BaseIntegrationTests(factory, testOutputHelper)
{
    [Fact]
    public async Task GetAccessTokenAsync_AdminRole_ShouldSuccess()
    {
        var request = new GetAccessTokenDto
        {
            Login = SeedConstants.AdminLogin,
            Password = SeedConstants.AdminPassword
        };

        var response = await PostAsync("api/auth", request);
        var tokensDto = await response.ToDtoAsync<JwtTokensDto>();

        tokensDto.AccessToken.ShouldNotBeNullOrEmpty();
        tokensDto.RefreshToken.ShouldNotBeNullOrEmpty();
    }
}