using HttpMcpServer3.Tools;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddRoutingCore();
builder.Services.AddHttpClient();

builder.Logging.AddConsole();
builder.Logging.AddSerilog();

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<ReportTools>();

var app = builder.Build();

app.MapMcp();

await app.RunAsync();
