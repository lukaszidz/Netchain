using System.Security.Cryptography;
using System.Text;
using Netchain.Core.Output;
using Newtonsoft.Json;


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
        var proof = 1;
        var block = CreateBlock(proof);
        return new CreatedBlock(block.Index, proof);
    }

    private Block CreateBlock(int proof)
    {
        var block = new Block(_chain.Count, DateTime.UtcNow, _chain.Count == 0 ? null : GetHash(_chain.First()), proof, _transactions);
        _transactions.Clear();
        _chain.AddFirst(block);
        return block;
    }
    private static string GetHash(Block block) => GetSha256(JsonConvert.SerializeObject(block));

    private static string GetSha256(string data)
    {
        var hashBuilder = new StringBuilder();

        byte[] bytes = Encoding.Unicode.GetBytes(data);
        byte[] hash = SHA256.HashData(bytes);

        foreach (var x in hash)
            hashBuilder.Append($"{x:x2}");

        return hashBuilder.ToString();
    }
}
