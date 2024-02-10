namespace Netchain.Server;

public readonly struct Peer(Guid id, string url)
{
    public Guid Id { get; } = id;
    public string Url { get; } = url;
}
