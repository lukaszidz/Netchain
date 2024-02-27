using System.Net;
using Netchain.Core;
using Netchain.Server.Constants;
using Netchain.Server.Responses;

namespace Netchain.Server.Tests.Api;

public sealed class ApiTests : IDisposable
{
    private readonly TestHttp _testHttp;

    public ApiTests()
    {
        _testHttp = new();
    }

    [Fact]
    public async Task Given_NewBlockchain_When_LastBlock_Then_ReturnGenesis()
    {
        // Arrange & Act
        var block = await _testHttp.Get<BlockResponse>(WebRoutes.LastBlock);

        // Assert
        Assert.NotNull(block);
        Assert.Equal(0, block.Index);
        Assert.NotEqual(default, block.Timestamp);
        Assert.NotEqual(default, block.Hash);
    }

    [Fact]
    public async Task Given_NewBlockchain_When_GetTransactions_Then_ReturnEmptyTransactions()
    {
        // Arrange & Act
        var transactions = await _testHttp.Get<IEnumerable<Transaction>>(WebRoutes.Transactions);

        // Assert
        Assert.Empty(transactions);
    }

    [Fact]
    public async Task Given_ExistingBlockchain_When_PostTransaction_Then_TransactionAdded()
    {
        // Arrange
        var transaction = new Transaction(Guid.NewGuid(), 12, Guid.NewGuid(), Guid.NewGuid());

        // Assert
        var response = await _testHttp.Post(WebRoutes.Transactions, transaction);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var transactions = await _testHttp.Get<IEnumerable<Transaction>>(WebRoutes.Transactions);
        Assert.Single(transactions);
        Assert.Contains(transaction, transactions);
    }

    [Fact]
    public async Task Given_AnyBlockchain_When_PostPeers_Then_ReturnSucccess()
    {
        // Arrange
        var body = new Peer("http://localhost:8001");

        // Act
        var response = await _testHttp.Post(WebRoutes.Peers, body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        _testHttp.Dispose();
    }
}
