namespace Netchain.Core;

public sealed class Blockchain
{
    private HashSet<Transaction> _transactions = new();
    private readonly LinkedList<Block> _chain = new();

    public Guid NodeId { get; } = Guid.NewGuid();
    public Block LastBlock => _chain.Last();

    public IEnumerable<Transaction> Transactions => _transactions;
    public IEnumerable<Block> Blocks => _chain;

    public Blockchain()
    {
        CreateBlock(0);
    }

    public Block Mine()
    {
        var proof = CreateProofOfWork(LastBlock.Proof, LastBlock.PreviousHash);
        var block = CreateBlock(proof);
        _transactions = new();
        return block;
    }

    public void CreateTransaction(Guid sender, Guid recipient, int amount)
    {
        var transaction = new Transaction(amount, sender, recipient);
        AppendTransaction(transaction);
    }

    public void AppendTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public void AppendBlock(Block block)
    {
        _chain.AddLast(block);
    }

    private Block CreateBlock(int proof)
    {
        var block = new Block(_chain.Count, DateTime.UtcNow, _chain.Count == 0 ? null : CryptoUtils.GetHash(_chain.Last()), proof, _transactions);
        AppendBlock(block);
        return block;
    }

    private static int CreateProofOfWork(int lastProof, string previousHash)
    {
        int proof = 0;
        while (!CryptoUtils.IsValidProof(lastProof, proof, previousHash))
            proof++;

        return proof;
    }
}
