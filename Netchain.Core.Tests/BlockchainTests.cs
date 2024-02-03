namespace Netchain.Core.Tests;

public class BlockchainTests
{
    private readonly Blockchain _blockchain;

    public BlockchainTests()
    {
        _blockchain = new Blockchain();
    }

    [Fact]
    public void Given_NewBlockchain_Then_EmptyStateInitialized()
    {
        // Assert
        Assert.NotEqual(Guid.Empty, _blockchain.NodeId);
        Assert.Empty(_blockchain.Transactions);
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
}