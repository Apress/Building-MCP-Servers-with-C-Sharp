using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ResourceServer.Resources;

[McpServerResourceType]
public static class ServerInfoResources
{
    [McpServerResource(
        UriTemplate = "info://server/status",
        Name = "Server Status",
        MimeType = "application/json")]
    [Description(
        "Returns the current server status " +
        "as JSON.")]
    public static string GetServerStatus()
    {
        var status = new
        {
            ServerName = "ResourceServer",
            Version = "1.0.0",
            Status = "Running",
            StartedAt = DateTime.UtcNow
                .ToString("o"),
            Environment =
                Environment.OSVersion.ToString(),
            DotNetVersion =
                Environment.Version.ToString()
        };

        return JsonSerializer.Serialize(
            status,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
    }

    // MimeType "text/markdown" tells clients this resource
    // contains Markdown — a capable client can render it with
    // formatting instead of showing raw text
    [McpServerResource(
        UriTemplate = "info://server/help",
        Name = "Server Help",
        MimeType = "text/markdown")]
    [Description(
        "Returns help documentation " +
        "for the resource server.")]
    public static string GetHelpDocument()
    {
        return """
            # ResourceServer Help

            This MCP server exposes several
            resources you can read.

            ## Available Resources

            - **cosmic-pizza://menu**
              The full Cosmic Pizza menu.
            - **cosmic-pizza://hours**
              Opening hours for all locations.
            - **planets://info/{planetName}**
              Info about a planet. Replace
              {planetName} with a planet name.
            - **info://server/status**
              Current server status as JSON.
            - **info://server/help**
              This help document.
            """;
    }
}
