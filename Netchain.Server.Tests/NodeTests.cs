using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Netchain.Server.Tests;

public sealed class NodeTests : IDisposable
{
    private readonly TestServer _node1;
    private readonly TestServer _node2;
    private readonly HttpClient _node1Client;
    private readonly HttpClient _node2Client;

    public NodeTests()
    {
        _node1 = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        _node2 = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        _node1Client = _node1.CreateClient();
        _node2Client = _node2.CreateClient();
    }

    public void Dispose()
    {
        _node1Client.Dispose();
        _node2Client.Dispose();
        _node1.Dispose();
        _node2.Dispose();
    }

    [Fact]
    public async Task Initial_Test()
    {
        var response1 = await _node1Client.PostAsync("/peers", new StringContent(""));
        var response2 = await _node2Client.PostAsync("/peers", new StringContent(""));
        var failing = await _node1Client.PostAsync("/peer", new StringContent(""));

        Assert.Equal(200, (int)response1.StatusCode);
        Assert.Equal(200, (int)response2.StatusCode);
        Assert.Equal(404, (int)failing.StatusCode);
    }
}
