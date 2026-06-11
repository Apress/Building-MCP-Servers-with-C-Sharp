using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

// Unlike Chapter 4's Host.CreateEmptyApplicationBuilder, we use
// WebApplication.CreateBuilder because HTTP transport needs
// ASP.NET Core's web server (Kestrel) to accept HTTP requests
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer()
    // WithHttpTransport() configures the server to communicate
    // over HTTP using streamable HTTP transport instead of
    // stdin/stdout. This enables remote clients to connect over
    // a network — the server runs as a web service rather than
    // a child process launched by the client
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// MapMcp() registers the MCP HTTP endpoint in the ASP.NET Core
// routing pipeline, creating the URL path where clients send
// JSON-RPC requests to interact with the server
app.MapMcp();

app.Run();

[McpServerToolType]
public static class EchoTool
{
    // The tool implementation is identical to the stdio version —
    // transport is a separate concern from tool logic
    [McpServerTool,
     Description("Echoes the input back.")]
    public static string Echo(
        [Description("Message to echo")]
        string message)
    {
        return $"Echo: {message}";
    }
}

[McpServerToolType]
public static class SpaceFactTool
{
    private static readonly string[] Facts =
    [
        "A day on Venus is longer than a year on Venus.",
        "Neutron stars are so dense that a teaspoon " +
            "would weigh about 6 billion tons.",
        "There are more stars in the universe than " +
            "grains of sand on all of Earth's beaches.",
        "The footprints on the Moon will be there " +
            "for 100 million years.",
        "One million Earths could fit inside the Sun."
    ];

    [McpServerTool, Description(
        "Returns a random fun fact about space.")]
    public static string GetSpaceFact()
    {
        return Facts[Random.Shared.Next(Facts.Length)];
    }
}
