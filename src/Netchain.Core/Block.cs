namespace Netchain.Core;

public sealed class Block
{
    private readonly LinkedList<Transaction> _transactions = new();

    public Block(int index, DateTime timestamp, string previousHash, int proof, LinkedList<Transaction> transactions)
    {
        Index = index;
        Timestamp = timestamp;
        Proof = proof;
        PreviousHash = previousHash;
        AddTransactions(transactions);
    }

    public int Index { get; }
    public DateTime Timestamp { get; }
    public int Proof { get; }
    public string PreviousHash { get; }
    public IEnumerable<Transaction> Transactions => _transactions;

    private void AddTransactions(LinkedList<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            _transactions.AddLast(transaction);
        }
    }
}
