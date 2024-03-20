using System.Collections.Concurrent;
using Netchain.Core;
using Netchain.Core.Events;
using Netchain.Server.Client;

namespace Netchain.Server;

public sealed class Node
{
    private readonly Blockchain _blockchain;
    private readonly ConcurrentDictionary<string, Peer> _peers = [];
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

    public async Task ConnectToPeers(IEnumerable<Peer> newPeers)
    {
        var tasks = newPeers.Select(async peer =>
        {
            if (!_peers.ContainsKey(peer.Url))
            {
                _peers[peer.Url] = peer;
                NotifyPeer(peer);

                try
                {
                    await Task.WhenAll(MergePeerLastBlock(peer), MergePeerTransactions(peer));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while merging with peer {PeerUrl}", peer.Url);
                }
            }
            else
            {
                _logger.LogInformation("The {SourceNode} already knows about the {TargetNode}", Address, peer.Url);
            }
        });

        await Task.WhenAll(tasks);
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

    private void NotifyPeer(Peer peer) => _nodeClient.Notify(new Peer(Address), peer);

    private async Task MergePeerLastBlock(Peer peer)
    {
        var response = await _nodeClient.GetLastBlock(peer);
        var lastBlock = Block.FromExisting(response.Index, response.Timestamp.Value, response.PreviousHash, response.Hash, response.Proof, response.Transactions.ToHashSet());
        MergeBlock(lastBlock);
    }

    private async Task MergePeerTransactions(Peer peer)
    {
        var transactions = await _nodeClient.GetTransactions(peer);
        foreach (var transaction in transactions)
        {
            _blockchain.AppendTransaction(transaction);
        }
    }
}
