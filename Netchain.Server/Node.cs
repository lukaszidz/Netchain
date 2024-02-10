using Netchain.Core;
using Netchain.Server.Client;

namespace Netchain.Server;

public sealed class Node
{
    private readonly Blockchain _blockchain;
    private readonly Dictionary<Guid, Peer> _peers;
    private readonly NodeClient _nodeClient;

    public Guid Id { get; } = Guid.NewGuid();
    public string Address { get; }

    public Node(string address, Blockchain blockchain, NodeClient nodeClient)
    {
        _blockchain = blockchain;
        _nodeClient = nodeClient;
        Address = address;
    }

    public async Task ConnectToPeers(IEnumerable<Peer> newPeers)
    {
        foreach (var peer in newPeers)
        {
            if (!_peers.ContainsKey(peer.Id))
            {
                await NotifyPeer(peer);
                GetLastBlock(peer);
                GetTransactions(peer);
                _peers[peer.Id] = peer;
            }
        }
    }

    private async Task NotifyPeer(Peer peer)
    {
        await _nodeClient.Notify(new Peer(Id, Address), peer);
    }

    private void GetLastBlock(Peer peer)
    {
        throw new NotImplementedException();
    }

    private void GetTransactions(Peer peer)
    {
        throw new NotImplementedException();
    }
}
