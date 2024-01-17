using BBCDisk.Commands;
using Cocona;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();

app.AddCommands<CatCommands>();

app.Run();