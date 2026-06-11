using CosmicPizzaServer.Models;
using CosmicPizzaServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(null);

builder.Services.Configure<CosmicPizzaOptions>(
    builder.Configuration.GetSection(
        CosmicPizzaOptions.SectionName));

builder.Services
    .AddSingleton<IPizzaMenuService,
        PizzaMenuService>();

builder.Services
    .AddSingleton<IOrderService, OrderService>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly()
    .WithPromptsFromAssembly();

await builder.Build().RunAsync();
