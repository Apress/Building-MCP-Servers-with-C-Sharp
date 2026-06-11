using DiServer.Models;
using DiServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Services.Configure<PizzaServerOptions>(
    builder.Configuration.GetSection(
        PizzaServerOptions.SectionName));

builder.Services
    .AddSingleton<IPizzaMenuService, PizzaMenuService>();

builder.Services.AddHttpClient();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
