using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMcpServer()
    // .WithHttpTransport(options =>
    // {
    //     // Stateless mode is recommended for servers that don't need
    //     // server-to-client requests like sampling or elicitation.
    //     // See the Sessions documentation for details.
    //     options.Stateless = true;
    //     // options.EventStreamStore = new InMemoryEventStreamStore();

    // })
    // .WithMessageFilters(messageFilters =>
    // {
    //     messageFilters.AddOutgoingFilter(next => async (context, cancellationToken) =>
    //     {
    //         var logger = context.Services?.GetService<ILogger<Program>>();

    //         // Inspect outgoing messages
    //         switch (context.JsonRpcMessage)
    //         {
    //             case JsonRpcResponse response:
    //                 logger?.LogInformation($"Sending response for request {response.Id}");
    //                 break;
    //             case JsonRpcNotification notification:
    //                 logger?.LogInformation($"Sending notification: {notification.Method}");
    //                 break;
    //         }

    //         await next(context, cancellationToken);
    //     });
    // })
    .WithHttpTransport()
    .WithToolsFromAssembly();
var app = builder.Build();

app.MapMcp("/sse");

app.Run("http://localhost:3001");

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}

// public class InMemoryEventStreamStore : ISseEventStreamStore
// {
//     public ValueTask<ISseEventStreamWriter> CreateStreamAsync(SseEventStreamOptions options, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }

//     public ValueTask<ISseEventStreamReader?> GetStreamReaderAsync(string lastEventId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
// }