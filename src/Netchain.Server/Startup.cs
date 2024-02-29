using Microsoft.AspNetCore.Mvc;
using Netchain.Core;
using Netchain.Server.Client;
using Netchain.Server.Constants;
using Netchain.Server.Responses;

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
            e.MapGet(WebRoutes.LastBlock, (Blockchain chain) =>
            {
                var lastBlock = chain.LastBlock;
                return Results.Ok(new BlockResponse
                {
                    Index = lastBlock.Index,
                    Timestamp = lastBlock.Timestamp,
                    Proof = lastBlock.Proof,
                    PreviousHash = lastBlock.PreviousHash,
                    Hash = lastBlock.Hash,
                    Transactions = lastBlock.Transactions
                });
            });
            e.MapPut(WebRoutes.LastBlock, (Node node, [FromBody] Block block) =>
            {
                node.MergeBlock(block);
                return Results.Ok();
            });
            e.MapPost(WebRoutes.Mine, (Blockchain chain) =>
            {
                chain.Mine();
                return Results.Created();
            });
            e.MapGet(WebRoutes.Transactions, (Blockchain chain) =>
            {
                var transactions = chain.Transactions.Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    Value = t.Value,
                    Sender = t.Sender,
                    Recipient = t.Recipient
                });
                return Results.Ok(transactions);
            });
            e.MapPost(WebRoutes.Transactions, (Blockchain chain, [FromBody] Transaction transaction) =>
            {
                chain.AppendTransaction(transaction);
                return Results.Created();
            });
            e.MapGet(WebRoutes.Peers, (Node node) =>
            {
                return Results.Ok(node.Peers);
            });
            e.MapPost(WebRoutes.Peers, (Node node, [FromBody] Peer peer) =>
            {
                node.ConnectToPeers([peer]);
                return Results.Ok();
            });
        });
    }
}
