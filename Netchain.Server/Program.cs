using Netchain.Core;
using Netchain.Server;
using Netchain.Server.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<NodeClient>();

builder.Services.AddSingleton<Blockchain>();
builder.Services.AddSingleton(sp =>
{
    var blockchain = sp.GetRequiredService<Blockchain>();
    var nodeClient = sp.GetRequiredService<NodeClient>();
    var logger = sp.GetRequiredService<ILogger<Node>>();

    var node = new Node(Environment.GetEnvironmentVariable("BaseUrl"), blockchain, nodeClient, logger);
    node.ConnectToPeers(Env.GetEnvironmentValues("Peers").Select(url => new Peer(url)));
    return node;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/peers", (Node node) =>
{
    return Results.Ok(node.Address);
})
.WithOpenApi();

app.Run();
