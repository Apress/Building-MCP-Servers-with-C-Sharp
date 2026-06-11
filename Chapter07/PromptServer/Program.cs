using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    // WithPromptsFromAssembly() scans this assembly for all
    // classes marked with [McpServerPromptType] and registers
    // their [McpServerPrompt] methods. Prompts are reusable
    // message templates that clients can request and fill in
    // with parameters — they guide AI models with pre-crafted
    // system instructions and user messages
    .WithPromptsFromAssembly();

await builder.Build().RunAsync();
