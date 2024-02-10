using System.Text;
using Newtonsoft.Json;

namespace Netchain.Server.Client;

public sealed class NodeClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task Notify(Peer source, Peer target)
    {
        await _httpClient.PostAsync($"{target.Url}/peers", new StringContent(JsonConvert.SerializeObject(source), Encoding.UTF8, @"application/json"));
    }
}
