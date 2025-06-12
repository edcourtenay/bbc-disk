using BBCDisk.Commands;
using BBCDisk.Services;
using Cocona;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<DiskHandler>();

var app = builder.Build();

app.AddCommands<CatCommands>();
app.AddCommands<DumpCommands>();
app.AddCommands<ExtractCommands>();

app.Run();