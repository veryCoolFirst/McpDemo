namespace SeeMcpServer.Tools;

using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"[sse-demo] hello {message}";
}