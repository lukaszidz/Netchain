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
    return new Node(builder.Configuration["BaseUrl"], blockchain, nodeClient);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/peers", (Node node) =>
{
    return Results.Ok(node.Address);
})
.WithOpenApi();

app.Run();

var blockchain = new Blockchain();
var node = new Node(app.Urls.First(), blockchain, app.Services.GetRequiredService<NodeClient>());
