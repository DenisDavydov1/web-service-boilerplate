using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using BoilerPlate.App.API;
using BoilerPlate.Core.Serialization;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;
using BoilerPlate.Tests.Base;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace BoilerPlate.Tests.Integration;

public abstract class BaseIntegrationTests : BaseDbTests, IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    protected BaseIntegrationTests(TestWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        factory.AddServices = AddHostServices;
        _client = factory.CreateClient();
    }

    protected virtual void AddHostServices(IServiceCollection services)
    {
    }

    protected async Task AuthorizeAsync(string login, string password)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/auth");
        var requestBody = new GetAccessTokenDto { Login = login, Password = password };
        request.Content = Serialize(requestBody);

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<JwtTokensDto>(responseContent, SerializationSettings.Default);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.AccessToken);
    }

    #region GET

    protected async Task<HttpResponseMessage> GetAsync(string uri) =>
        await _client.GetAsync(uri);

    #endregion

    #region DELETE

    protected async Task<HttpResponseMessage> DeleteAsync(string uri) =>
        await _client.DeleteAsync(uri);

    #endregion

    #region POST

    protected async Task<HttpResponseMessage> PostAsync(string uri) =>
        await _client.PostAsync(uri, null);

    protected async Task<HttpResponseMessage> PostAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PostAsync(uri, Serialize(command));

    protected async Task<HttpResponseMessage> PostAsync(string uri, HttpContent httpContent) =>
        await _client.PostAsync(uri, httpContent);

    #endregion

    #region PUT

    protected async Task<HttpResponseMessage> PutAsync(string uri) =>
        await _client.PutAsync(uri, null);

    protected async Task<HttpResponseMessage> PutAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PutAsync(uri, Serialize(command));

    protected async Task<HttpResponseMessage> PutAsync(string uri, HttpContent httpContent) =>
        await _client.PutAsync(uri, httpContent);

    #endregion

    #region PATCH

    protected async Task<HttpResponseMessage> PatchAsync(string uri) =>
        await _client.PatchAsync(uri, null);

    protected async Task<HttpResponseMessage> PatchAsync<TCommand>(string uri, TCommand command)
        where TCommand : class =>
        await _client.PatchAsync(uri, Serialize(command));

    protected async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent httpContent) =>
        await _client.PatchAsync(uri, httpContent);

    #endregion

    private static StringContent Serialize<TCommand>(TCommand command) where TCommand : class =>
        new (JsonConvert.SerializeObject(command, SerializationSettings.Default), Encoding.UTF8, MediaTypeNames.Application.Json);
}