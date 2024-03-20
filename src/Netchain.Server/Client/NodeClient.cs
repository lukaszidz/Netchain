using Netchain.Core;
using Netchain.Server.Constants;
using Netchain.Server.Responses;

namespace Netchain.Server.Client;

public sealed class NodeClient(HttpClient httpClient, ILogger<NodeClient> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<NodeClient> _logger = logger;

    public Task Notify(Peer source, Peer target)
    {
        _logger.LogInformation("Started notifying the peer {Url}", target.Url);
        return Post($"{target.Url}/{WebRoutes.Peers}", source);
    }

    public Task<BlockResponse> GetLastBlock(Peer peer)
    {
        _logger.LogInformation("Getting the latest block of the peer {Url}", peer.Url);
        return Get<BlockResponse>($"{peer.Url}/{WebRoutes.LastBlock}");
    }

    public Task SendLastBlock(Peer peer, Block block)
    {
        _logger.LogInformation("Sending the latest block for the peer {Url}", peer.Url);
        return Post($"{peer.Url}/{WebRoutes.LastBlock}", block);
    }

    public Task SendTransaction(Peer peer, Transaction transaction)
    {
        _logger.LogInformation("Sending the transaction for the peer {Url}", peer.Url);
        return Post($"{peer.Url}/{WebRoutes.Transactions}", transaction);
    }

    public Task<IEnumerable<Transaction>> GetTransactions(Peer peer)
    {
        _logger.LogInformation("Getting the transactions of the peer {Url}", peer.Url);
        return Get<IEnumerable<Transaction>>($"{peer.Url}/{WebRoutes.Transactions}");
    }

    private Task<T> Get<T>(string url) => _httpClient.GetFromJsonAsync<T>(url);
    private Task Post<T>(string url, T body) => _httpClient.PostAsJsonAsync(url, body);
    private Task Put<T>(string url, T body) => _httpClient.PutAsJsonAsync(url, body);
}
