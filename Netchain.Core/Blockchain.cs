using Netchain.Core.Output;

namespace Netchain.Core;

public sealed class Blockchain
{
    private readonly LinkedList<Transaction> _transactions = new();
    private readonly LinkedList<Block> _chain = new();

    public Guid NodeId { get; } = Guid.NewGuid();

    public ICollection<Transaction> Transactions => _transactions;
    public ICollection<Block> Blocks => _chain;

    public Blockchain()
    {
        CreateBlock(0);
    }

    public void CreateTransaction(Guid sender, Guid recipient, int amount)
    {
        var transaction = new Transaction(amount, sender, recipient);
        _transactions.AddLast(transaction);
    }

    public CreatedBlock Mine()
    {
        var lastBlock = _chain.Last();
        var proof = CreateProofOfWork(lastBlock.Proof, lastBlock.PreviousHash);
        var block = CreateBlock(proof);
        _transactions.Clear();
        return block;
    }

    private CreatedBlock CreateBlock(int proof)
    {
        var block = new Block(_chain.Count, DateTime.UtcNow, _chain.Count == 0 ? null : CryptoUtils.GetHash(_chain.First()), proof, _transactions);
        _chain.AddFirst(block);
        return new CreatedBlock(block.Index, proof, block.Transactions.Select(t => t.Id).ToList());
    }

    private static int CreateProofOfWork(int lastProof, string previousHash)
    {
        int proof = 0;
        while (!CryptoUtils.IsValidProof(lastProof, proof, previousHash))
            proof++;

        return proof;
    }
}
