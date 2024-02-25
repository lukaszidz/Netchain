using Netchain.Core;

namespace Netchain.Server.Tests.Api;

public sealed class ApiTests
{
    [Fact]
    public async Task Given_NewBlockchain_When_LastBlock_Then_ReturnGenesis()
    {
        // Arrange & Act
        var block = await TestHttp.Get<Block>("/blockchain/block/last");

        // Assert
        Assert.NotNull(block);
        Assert.Equal(0, block.Index);
    }

    [Fact]
    public async Task Given_NewBlockchain_When_Transactions_Then_ReturnEmptyTransactions()
    {
        // Arrange & Act
        var transactions = await TestHttp.Get<IEnumerable<Transaction>>("/blockchain/transactions");

        // Assert
        Assert.Empty(transactions);
    }

    [Fact]
    public async Task Given_AnyBlockchain_When_Peers_Then_ReturnSucccess()
    {
        // Arrange
        var body = new Peer("http://localhost:8001");

        // Act
        var response = await TestHttp.Post("/node/peers", body);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
}
