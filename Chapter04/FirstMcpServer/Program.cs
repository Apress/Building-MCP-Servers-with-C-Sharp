using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

// Host.CreateEmptyApplicationBuilder gives us dependency injection,
// configuration, and logging — the same foundation used by
// ASP.NET Core, reused here for a console-based MCP server
var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Services
    // AddMcpServer() registers the core MCP server services
    // (protocol handling, capability negotiation, and JSON-RPC
    // message processing) into the dependency injection container
    .AddMcpServer()
    // WithStdioServerTransport() tells the server to communicate
    // over stdin/stdout, which is how MCP clients like Claude
    // Desktop and VS Code launch and talk to local tool servers
    .WithStdioServerTransport()
    // WithToolsFromAssembly() uses reflection to scan this
    // assembly for all classes marked with [McpServerToolType]
    // and registers their [McpServerTool] methods automatically
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

// [McpServerToolType] marks this class as a container for MCP
// tools — the SDK uses this attribute during assembly scanning
// to know which classes to inspect for tool methods
[McpServerToolType]
public static class EchoTool
{
    // [McpServerTool] exposes this method as a callable tool
    // in the MCP protocol. The [Description] attributes provide
    // metadata that AI models use to understand when and how
    // to invoke the tool, and what each parameter expects
    [McpServerTool, Description("Echoes the input back.")]
    public static string Echo(
        [Description("Message to echo")] string message)
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
        "One million Earths could fit inside the Sun.",
        "Space is completely silent because there is " +
            "no atmosphere to carry sound.",
        "The Milky Way galaxy is about 100,000 " +
            "light-years in diameter.",
        "A full NASA space suit costs approximately " +
            "$12 million.",
        "Saturn's density is low enough that it would " +
            "float in water.",
        "The International Space Station travels at " +
            "about 28,000 kilometers per hour."
    ];

    [McpServerTool, Description(
        "Returns a random fun fact about space.")]
    public static string GetSpaceFact()
    {
        return Facts[Random.Shared.Next(Facts.Length)];
    }
}
