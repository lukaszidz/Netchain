using Netchain.Core;

namespace Netchain.Server.Responses;

public sealed class BlockResponse
{
    public int Index { get; set; }
    public DateTime? Timestamp { get; set; }
    public int Proof { get; set; }
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
}
