using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

var subscriptions =
    new ConcurrentDictionary<string, int>();

var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    // WithResourcesFromAssembly() scans this assembly for all
    // classes marked with [McpServerResourceType] and registers
    // their [McpServerResource] methods as readable resources —
    // unlike tools (which perform actions), resources expose
    // data that clients can read on demand
    .WithResourcesFromAssembly()
    .WithSubscribeToResourcesHandler(
    async (ctx, ct) =>
    {
        var uri = ctx.Params?.Uri;
        if (uri is not null)
        {
            // Track this subscription
            subscriptions.TryAdd(uri, 0);
        }
        return new EmptyResult();
    })
    .WithUnsubscribeFromResourcesHandler(
    async (ctx, ct) =>
    {
        var uri = ctx.Params?.Uri;
        if (uri is not null)
        {
            subscriptions.TryRemove(
                uri, out _);
        }
        return new EmptyResult();
    });

await builder.Build().RunAsync();
