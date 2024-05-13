using System.Diagnostics;
using System.Text;
using BoilerPlate.Core.Concurrency;
using ManagedCode.MimeTypes;

namespace BoilerPlate.Core.Http;

public class HttpHandler : IDisposable
{
    private HttpClient _httpClient = new();
    private readonly Stopwatch _stopwatch = new();
    private readonly SemaphoreLocker _semaphoreLocker = new();

    public Dictionary<string, string> Headers { get; set; } = new();
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    public void Dispose() => _httpClient.Dispose();

    public HttpHandler PreventRedirect()
    {
        _httpClient.Dispose();

        _httpClient = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false
        });

        return this;
    }

    public HttpHandler Timeout(int seconds)
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
        return this;
    }

    public async Task<HttpResponseMessage> GetAsync(string uri, CancellationToken ct = default)
    {
        using var request = CreateRequestWithHeaders();

        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri(uri);

        return await SendAndTrackTime(request, ct);
    }

    public async Task<HttpResponseMessage> PostAsync(string uri, string content, CancellationToken ct = default)
    {
        using var request = CreateRequestWithHeaders();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(uri);
        request.Content = new StringContent(content, Encoding.UTF8, MimeHelper.JSON);

        return await SendAndTrackTime(request, ct);
    }

    public async Task<HttpResponseMessage> PostAsync(
        string uri,
        IEnumerable<(dynamic content, string name, string? fileName)> multipartContent,
        CancellationToken ct = default)
    {
        var content = new MultipartFormDataContent();

        foreach (var part in multipartContent)
        {
            switch (part)
            {
                case { content: string stringContent, fileName: null }:
                    content.Add(new StringContent(stringContent, Encoding.UTF8, MimeHelper.JSON), part.name);
                    break;
                case { content: byte[] binaryContent, fileName: not null }:
                    content.Add(new StreamContent(new MemoryStream(binaryContent)), part.name, part.fileName);
                    break;
                default:
                    throw new ArgumentException($"Invalid multipart data {part.name}");
            }
        }

        using var request = CreateRequestWithHeaders();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(uri);
        request.Content = content;

        return await SendAndTrackTime(request, ct);
    }

    private HttpRequestMessage CreateRequestWithHeaders()
    {
        var request = new HttpRequestMessage();

        foreach (var header in Headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        return request;
    }

    private async Task<HttpResponseMessage> SendAndTrackTime(HttpRequestMessage request, CancellationToken ct) =>
        await _semaphoreLocker.LockAsync(async () =>
        {
            _stopwatch.Restart();
            var response = await _httpClient.SendAsync(request, ct);
            _stopwatch.Stop();

            return response;
        });
}