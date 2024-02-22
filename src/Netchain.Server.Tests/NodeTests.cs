using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

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
        // Arrange
        var body = new StringContent(JsonConvert.SerializeObject(new Peer("http://localhost:8001")), Encoding.UTF8, "application/json");
        var response1 = await _node1Client.PostAsync("/node/peers", body);
        var response2 = await _node2Client.PostAsync("/node/peers", body);
        var failing = await _node1Client.PostAsync("/node/peer", body);

        Assert.Equal(200, (int)response1.StatusCode);
        Assert.Equal(200, (int)response2.StatusCode);
        Assert.Equal(404, (int)failing.StatusCode);

    }
}
