using Netchain.Core;
using Netchain.Core.Events;
using Netchain.Server.Client;

namespace Netchain.Server;

public sealed class Node
{
    private readonly Blockchain _blockchain;
    private readonly Dictionary<string, Peer> _peers = [];
    private readonly NodeClient _nodeClient;
    private readonly ILogger<Node> _logger;

    public IEnumerable<Peer> Peers => _peers.Select(p => p.Value);

    public Guid Id { get; } = Guid.NewGuid();
    public string Address { get; }

    public Node(string address, Blockchain blockchain, NodeClient nodeClient, ILogger<Node> logger)
    {
        _blockchain = blockchain;
        _nodeClient = nodeClient;
        _logger = logger;
        Address = address;
        SubscribeBlockchain();
    }

    public async void ConnectToPeers(IEnumerable<Peer> newPeers)
    {
        foreach (var peer in newPeers)
        {
            if (!_peers.ContainsKey(peer.Url))
            {
                _peers[peer.Url] = peer;
                NotifyPeer(peer);
                await MergePeerLastBlock(peer);
                await MergePeerTransactions(peer);
            }
            else
            {
                _logger.LogInformation("The {SourceNode} already knows about the {TargetNode}", Address, peer.Url);
            }
        }
    }

    public void MergeBlock(Block block)
    {
        _blockchain.MergeBlock(block);
    }

    public void MergeTransaction(Transaction transaction)
    {
        _blockchain.AppendTransaction(transaction);
    }

    private void SubscribeBlockchain()
    {
        _blockchain.BlockAdded += PublishLastBlock;
        _blockchain.TransactionAdded += PublishTransaction;
    }

    private void PublishLastBlock(object sender, BlockAdded e)
    {
        var tasks = Peers.Select(p => _nodeClient.SendLastBlock(p, e.Block));
        Task.WaitAll(tasks.ToArray());
    }

    private void PublishTransaction(object sender, TransactionAdded e)
    {
        var tasks = Peers.Select(p => _nodeClient.SendTransaction(p, e.Transaction));
        Task.WaitAll(tasks.ToArray());
    }

    private void NotifyPeer(Peer peer)
    {
        _nodeClient.Notify(new Peer(Address), peer);
    }

    private async Task MergePeerLastBlock(Peer peer)
    {
        var lastBlock = await _nodeClient.GetLastBlock(peer);
        MergeBlock(lastBlock);
    }

    private async Task MergePeerTransactions(Peer peer)
    {
        var transactions = await _nodeClient.GetTransactions(peer);
        foreach (var transaction in transactions)
        {
            if (!_blockchain.Transactions.Contains(transaction))
            {
                _logger.LogInformation("Received transaction {TransactionId} has beed added to the blockchain", transaction.Id);
                _blockchain.AppendTransaction(transaction);
            }
        }
    }
}
