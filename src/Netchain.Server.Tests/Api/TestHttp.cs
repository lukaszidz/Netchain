using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace Netchain.Server.Tests.Api;

internal static class TestHttp
{
    public static async Task<T> Get<T>(string uri)
    {
        using var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        using var client = server.CreateClient();
        var response = await client.GetAsync(uri);
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }
    public static async Task<HttpResponseMessage> Post(string uri, object body)
    {
        using var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        using var client = server.CreateClient();
        return await client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
    }
}
