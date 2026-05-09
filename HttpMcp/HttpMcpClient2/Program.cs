using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Everything",
    Command = "dotnet run",
    Arguments = ["--project", @"E:\Tiancity\DevCode\McpDemo\HttpMcp\HttpMcpServer\HttpMcpServer.csproj"],
});

var client = await McpClient.CreateAsync(clientTransport);

// Print the list of tools available from the server.
foreach (var tool in await client.ListToolsAsync())
{
    Console.WriteLine($"{tool.Name} ({tool.Description}) \n {tool.JsonSchema}");
}


var result2 = await client.CallToolAsync(
    "getrandomnumber",
    new Dictionary<string, object?>() { ["min"] = 10, ["max"] = 100 },
    cancellationToken: CancellationToken.None);

Console.WriteLine(result2.Content.OfType<TextContentBlock>().First().Text);