using Microsoft.AspNetCore.Mvc;
using Netchain.Core;
using Netchain.Server.Client;

namespace Netchain.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient<NodeClient>();
        services.AddSingleton<Blockchain>();

        services.AddSingleton(sp =>
        {
            var blockchain = sp.GetRequiredService<Blockchain>();
            var nodeClient = sp.GetRequiredService<NodeClient>();
            var logger = sp.GetRequiredService<ILogger<Node>>();

            var node = new Node(Environment.GetEnvironmentVariable("BaseUrl"), blockchain, nodeClient, logger);
            node.ConnectToPeers(Env.GetEnvironmentValues("Peers").Select(url => new Peer(url)));
            return node;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseEndpoints(e =>
        {
            e.MapGet("/blockchain/block/last", (Blockchain chain) =>
            {
                return Results.Ok(chain.LastBlock);
            });
            e.MapGet("/node/peers", (Node node) =>
            {
                return Results.Ok(node.Peers);
            });
            e.MapPost("/node/peers", (Node node, [FromBody] Peer peer) =>
            {
                node.ConnectToPeers([peer]);
                return Results.Ok();
            }).WithOpenApi();
        });
    }
}
