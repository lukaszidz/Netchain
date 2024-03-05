using Netchain.Core;

namespace Netchain.Server.Requests;

public sealed class AddBlockRequest
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public int Proof { get; set; }
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
    public HashSet<Transaction> Transactions { get; set; }
}
