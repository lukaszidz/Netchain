using System.Text;
using System.Text.Json;
using Netchain.Core;
using Netchain.Server.Constants;

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
                await _httpClient.PostAsync($"{target.Url}/{WebRoutes.Peers}", new StringContent(JsonSerializer.Serialize(source), Encoding.UTF8, @"application/json"));
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
        return Get<Block>($"{peer.Url}/{WebRoutes.LastBlock}");
    }

    public Task<IEnumerable<Transaction>> GetTransactions(Peer peer)
    {
        _logger.LogInformation("Getting the transactions of the peer {Url}", peer.Url);
        return Get<IEnumerable<Transaction>>($"{peer.Url}/{WebRoutes.Transactions}");
    }

    private async Task<T> Get<T>(string url) => await _httpClient.GetFromJsonAsync<T>(url);
}
