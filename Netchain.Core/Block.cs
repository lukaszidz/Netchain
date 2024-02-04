namespace Netchain.Core;

public sealed class Block(int index, DateTime timestamp, string previousHash, int proof, LinkedList<Transaction> transactions)
{
    private readonly LinkedList<Transaction> _transactions = transactions;

    public int Index { get; } = index;
    public DateTime Timestamp { get; } = timestamp;
    public int Proof { get; } = proof;
    public string PreviousHash { get; } = previousHash;
    public IEnumerable<Transaction> Transactions => _transactions;
}
