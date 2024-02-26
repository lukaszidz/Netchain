namespace Netchain.Core;

public sealed class Block
{
    private readonly HashSet<Transaction> _transactions = new();

    public Block(int index, DateTime timestamp, string previousHash, int proof, HashSet<Transaction> transactions)
    {
        _transactions = transactions;
        Index = index;
        Timestamp = timestamp;
        Proof = proof;
        PreviousHash = previousHash;
        Hash = CryptoUtils.GetHash(this);
    }

    public int Index { get; }
    public DateTime? Timestamp { get; }
    public int Proof { get; }
    public string Hash { get; }
    public string PreviousHash { get; }
    public IEnumerable<Transaction> Transactions => _transactions;
}
