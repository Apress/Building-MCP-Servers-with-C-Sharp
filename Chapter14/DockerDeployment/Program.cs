using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;

var builder = Host.CreateEmptyApplicationBuilder(null);

// AddMemoryCache() registers IMemoryCache in the DI container,
// which the SDK will automatically inject into tool methods
// that declare it as a parameter — this avoids redundant work
// when tools are called repeatedly with the same inputs
builder.Services.AddMemoryCache();

// Production MCP servers use the same builder pattern — the
// Docker container simply provides the runtime environment;
// the server code itself remains unchanged from development
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();
await app.RunAsync();
