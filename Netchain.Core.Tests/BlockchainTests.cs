using System.Security.Cryptography;
using System.Text;

namespace Netchain.Core.Tests;

public class BlockchainTests
{
    private readonly Blockchain _blockchain;

    public BlockchainTests()
    {
        _blockchain = new Blockchain();
    }

    [Fact]
    public void Given_NewBlockchain_Then_GenesisStateInitialized()
    {
        // Assert
        Assert.NotEqual(Guid.Empty, _blockchain.NodeId);
        Assert.Empty(_blockchain.Transactions);
        Assert.Single(_blockchain.Blocks);
    }

    [Fact]
    public void Given_ExistingBlockchain_When_CreateTransaction_Then_TransactionIsAdded()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var amount = 100;

        // Act
        _blockchain.CreateTransaction(senderId, recipientId, amount);

        // Assert
        Assert.Single(_blockchain.Transactions);
        var transaction = _blockchain.Transactions.Last();
        Assert.Equal(amount, transaction.Value);
        Assert.Equal(senderId, transaction.Sender);
        Assert.Equal(recipientId, transaction.Recipient);
    }

    [Fact]
    public void Given_ExistingBlockchain_When_Mine_Then_BlockIsAdded()
    {
        // Arrange
        var oldCount = _blockchain.Blocks.Count;

        // Act
        var createdBlock = _blockchain.Mine();

        //Assert
        Assert.Empty(_blockchain.Transactions);
        Assert.Equal(oldCount + 1, _blockchain.Blocks.Count);
        Assert.Equal(oldCount, createdBlock.Index);
    }

    [Fact]
    public void Given_ExistingBlockchain_When_Mine_Then_BlockIsAddedWithCorrectProof()
    {
        // Arrange
        var lastBlock = _blockchain.Blocks.Last();

        // Act
        var createdBlock = _blockchain.Mine();

        // Assert
        Assert.True(IsValidProof(lastBlock.Proof, createdBlock.Proof, lastBlock.PreviousHash));
    }

    private bool IsValidProof(int lastProof, int proof, string previousHash)
    {
        string guess = $"{lastProof}{proof}{previousHash}";
        string result = GetSha256(guess);
        return result.StartsWith("00");
    }

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
