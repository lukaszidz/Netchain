namespace Netchain.Server;

public readonly struct Peer(string url)
{
    public string Url { get; } = url;
}
