using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Everything",
    Command = "dotnet run",
    Arguments = ["--project", @"E:\Tiancity\DevCode\McpDemo\McpServer\McpServer.csproj"],
});

var client = await McpClient.CreateAsync(clientTransport);

// Print the list of tools available from the server.
foreach (var tool in await client.ListToolsAsync())
{
    Console.WriteLine($"{tool.Name} ({tool.Description}) \n {tool.JsonSchema}");
}

// Execute a tool (this would normally be driven by LLM tool invocations).
var result = await client.CallToolAsync(
    "echo",
    new Dictionary<string, object?>() { ["message"] = "Hello MCP!" },
    cancellationToken: CancellationToken.None);

// echo always returns one and only one text content object
Console.WriteLine(result.Content.OfType<TextContentBlock>().First().Text);


// var result2 = await client.CallToolAsync(
//     "random",
//     new Dictionary<string, object?>() { ["min"] = 10, ["max"] = 100 },
//     cancellationToken: CancellationToken.None);

// Console.WriteLine(result2.Content.OfType<TextContentBlock>().First().Text);