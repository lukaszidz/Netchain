namespace Netchain.Core.Tests;

public sealed class BlockchainTests
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
        Assert.True(CryptoUtils.IsValidProof(lastBlock.Proof, createdBlock.Proof, lastBlock.PreviousHash));
    }

    [Fact]
    public void Given_ExistingBlockchain_When_Mine_Then_TransactionsAreMovedToBlock()
    {
        // Arrange
        _blockchain.CreateTransaction(Guid.NewGuid(), Guid.NewGuid(), 10);
        _blockchain.CreateTransaction(Guid.NewGuid(), Guid.NewGuid(), 12);
        var transactions = _blockchain.Transactions.Select(t => t.Id).ToList();

        // Act
        var createdBlock = _blockchain.Mine();

        // Assert
        Assert.NotEmpty(createdBlock.TransactionIds);
        Assert.Equal(transactions, createdBlock.TransactionIds);
    }
}
