namespace Netchain.Core;

public sealed class Block
{
    private readonly ISet<Transaction> _transactions = new HashSet<Transaction>();

    private Block(ISet<Transaction> transactions)
    {
        _transactions = transactions;
    }

    public Block(int index, DateTime timestamp, string previousHash, int proof, HashSet<Transaction> transactions) : this(transactions)
    {
        Index = index;
        Timestamp = timestamp;
        Proof = proof;
        PreviousHash = previousHash;
        Hash = CryptoUtils.GetHash(this);
    }

    public static Block FromExisting(int index, DateTime timestamp, string previousHash, string hash, int proof, ISet<Transaction> transactions) =>
        new(transactions)
        {
            Index = index,
            Timestamp = timestamp,
            Proof = proof,
            PreviousHash = previousHash,
            Hash = hash
        };

    public int Index { get; private set; }
    public DateTime? Timestamp { get; private set; }
    public int Proof { get; private set; }
    public string Hash { get; private set; }
    public string PreviousHash { get; private set; }
    public IEnumerable<Transaction> Transactions => _transactions;
}
