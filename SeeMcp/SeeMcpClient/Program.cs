using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

// var endpoint = Environment.GetEnvironmentVariable("ENDPOINT") ?? "http://localhost:3001/sse";
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT") ?? "http://localhost:3001";

var clientTransport = new HttpClientTransport(new()
{
    Endpoint = new Uri(endpoint),
    // TransportMode = HttpTransportMode.Sse,
    TransportMode = HttpTransportMode.StreamableHttp
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