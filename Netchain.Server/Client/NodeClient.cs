using System.Text;
using Newtonsoft.Json;

namespace Netchain.Server.Client;

public sealed class NodeClient(HttpClient httpClient, ILogger<NodeClient> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<NodeClient> _logger = logger;

    public async void Notify(Peer source, Peer target)
    {
        await Task.Run(async () =>
        {
            try
            {
                await _httpClient.PostAsync($"{target.Url}/peers", new StringContent(JsonConvert.SerializeObject(source), Encoding.UTF8, @"application/json"));
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Couldn't notify the {Peer}. {Message}", target.Url, ex.Message);
            }
        });
    }
}
