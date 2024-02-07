namespace Netchain.Core.Output;

public readonly struct CreatedBlock(int index, int proof, IEnumerable<Guid> transactions)
{
    public int Index { get; } = index;
    public int Proof { get; } = proof;
    public IEnumerable<Guid> TransactionIds { get; } = transactions;
}
