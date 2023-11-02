using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using BoilerPlate.App.API;
using BoilerPlate.Core.Serialization;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;
using BoilerPlate.Data.Seeds.Constants;
using BoilerPlate.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Integration;

public abstract class BaseIntegrationTests : BaseDbTests, IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    protected BaseIntegrationTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    protected async Task AuthorizeAsync(UserRole role)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "auth");

        var requestBody = role switch
        {
            UserRole.User => new GetAccessTokenDto {Login = SeedConstants.UserLogin, Password = SeedConstants.UserPassword },
            UserRole.Admin => new GetAccessTokenDto { Login = SeedConstants.AdminLogin, Password = SeedConstants.AdminPassword },
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
        request.Content = Serialize(requestBody);

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<JwtTokensDto>(responseContent, SerializationSettings.Default);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.AccessToken);
    }

    protected async Task<HttpResponseMessage> GetAsync(string uri) =>
        await _client.GetAsync(uri);

    protected async Task<HttpResponseMessage> DeleteAsync(string uri) =>
        await _client.DeleteAsync(uri);

    protected async Task<HttpResponseMessage> PostAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PostAsync(uri, Serialize(command));

    protected async Task<HttpResponseMessage> PutAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PutAsync(uri, Serialize(command));

    protected async Task<HttpResponseMessage> PatchAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PatchAsync(uri, Serialize(command));

    private static StringContent Serialize<TCommand>(TCommand command) where TCommand : class =>
        new (JsonConvert.SerializeObject(command, SerializationSettings.Default), Encoding.UTF8, MediaTypeNames.Application.Json);
}