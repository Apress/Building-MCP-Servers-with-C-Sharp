using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

[McpServerToolType]
public static class ComplexReturnTools
{
    [McpServerTool(Name = "get_system_info")]
    [Description("Returns system information.")]
    public static object GetSystemInfo()
    {
        return new
        {
            MachineName = Environment.MachineName,
            OsVersion =
                Environment.OSVersion.ToString(),
            ProcessorCount =
                Environment.ProcessorCount,
            DotNetVersion =
                Environment.Version.ToString(),
            CurrentTime =
                DateTime.UtcNow.ToString("o")
        };
    }

    [McpServerTool(Name = "get_server_status")]
    [Description(
        "Returns the server status " +
        "with full control.")]
    public static CallToolResult GetServerStatus()
    {
        var status = new
        {
            Status = "Healthy",
            Uptime = "3 hours, 42 minutes",
            ToolsLoaded = 8,
            RequestsHandled = 156
        };

        var json = JsonSerializer.Serialize(
            status,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        return new CallToolResult
        {
            Content =
            [
                new TextContentBlock
                {
                    Text = json
                }
            ],
            IsError = false
        };
    }
}
