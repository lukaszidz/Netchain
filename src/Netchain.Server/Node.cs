using Netchain.Core;
using Netchain.Server.Client;

namespace Netchain.Server;

public sealed class Node
{
    private readonly Blockchain _blockchain;
    private readonly Dictionary<string, Peer> _peers = [];
    private readonly NodeClient _nodeClient;
    private readonly ILogger _logger;

    public IEnumerable<Peer> Peers => _peers.Select(p => p.Value);

    public Guid Id { get; } = Guid.NewGuid();
    public string Address { get; }

    public Node(string address, Blockchain blockchain, NodeClient nodeClient, ILogger<Node> logger)
    {
        _blockchain = blockchain;
        _nodeClient = nodeClient;
        _logger = logger;
        Address = address;
    }

    public async void ConnectToPeers(IEnumerable<Peer> newPeers)
    {
        foreach (var peer in newPeers)
        {
            if (!_peers.ContainsKey(peer.Url))
            {
                _peers[peer.Url] = peer;
                NotifyPeer(peer);
                var lastBlock = await _nodeClient.GetLastBlock(peer);
                //VerifyReceivedBlock(lastBlock);
                // GetTransactions(peer);
            }
            else
            {
                _logger.LogInformation("The {SourceNode} already knows about the {TargetNode}", Address, peer.Url);
            }
        }
    }

    private void NotifyPeer(Peer peer)
    {
        _nodeClient.Notify(new Peer(Address), peer);
    }

    private void VerifyReceivedBlock(Block block)
    {
        throw new NotImplementedException();
    }

    private void GetTransactions(Peer peer)
    {
        throw new NotImplementedException();
    }
}