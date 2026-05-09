using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    // .WithToolsFromAssembly(); // 通过反射获取工具
    // .WithTools<EchoTool2>(); // 通过依赖注入注册工具
    .WithTools(new List<Type> { typeof(EchoTool2), typeof(TestTool3) }.AsEnumerable());
await builder.Build().RunAsync();

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"[demo] hello {message}";
}

public class EchoTool2(IHostEnvironment env)
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public string Echo(string message) => $"[demo] hello {message}, env: {env.EnvironmentName}";
}

public class TestTool3
{
    [McpServerTool, Description("random number generator")]
    public string Random([Description("random min limit")] int min, [Description("random max limit, no required, default: 100")] int? max)
    {
        var result = new Random().Next(min, max ?? 100);
        return $"[demo] random result:{result}";
    }
}