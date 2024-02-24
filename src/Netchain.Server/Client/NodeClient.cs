using System.Text;
using Netchain.Core;
using Newtonsoft.Json;

namespace Netchain.Server.Client;

public sealed class NodeClient(HttpClient httpClient, ILogger<NodeClient> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<NodeClient> _logger = logger;

    public async void Notify(Peer source, Peer target)
    {
        _logger.LogInformation("Started notifying the peer {Url}", target.Url);

        await Task.Run(async () =>
        {
            try
            {
                await _httpClient.PostAsync($"{target.Url}/node/peers", new StringContent(JsonConvert.SerializeObject(source), Encoding.UTF8, @"application/json"));
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Couldn't notify the {Peer}. {Message}", target.Url, ex.Message);
            }
        });
    }

    public Task<Block> GetLastBlock(Peer peer)
    {
        _logger.LogInformation("Getting the latest block of the peer {Url}", peer.Url);
        return Get<Block>($"{peer.Url}/blockchain/block/last");
    }

    public Task<IEnumerable<Transaction>> GetTransactions(Peer peer)
    {
        _logger.LogInformation("Getting the transactions of the peer {Url}", peer.Url);
        return Get<IEnumerable<Transaction>>($"{peer.Url}/blockchain/transactions");
    }

    private async Task<T> Get<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        throw new HttpRequestException($"Request failed with status code {response}");
    }
}
