using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Netchain.Server.Tests.Api;

public sealed class TestHttp : IDisposable
{
    private readonly TestServer _server;

    public TestHttp()
    {
        _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
    }

    public async Task<T> Get<T>(string uri)
    {
        using var client = _server.CreateClient();
        var response = await client.GetAsync(uri);
        return await client.GetFromJsonAsync<T>(uri);
    }

    public async Task<HttpResponseMessage> Post(string uri, object body)
    {
        using var client = _server.CreateClient();
        return await client.PostAsJsonAsync(uri, body);
    }

    public void Dispose()
    {
        _server.Dispose();
    }
}
