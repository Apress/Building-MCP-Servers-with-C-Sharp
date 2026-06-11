using System.IO.Pipelines;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ServerIntegrationTests
{
    [TestMethod]
    public async Task CanCallToolViaProtocol()
    {
        var clientToServer = new Pipe();
        var serverToClient = new Pipe();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(
                clientToServer.Reader.AsStream(),
                serverToClient.Writer.AsStream())
            .WithToolsFromAssembly();

        var host = builder.Build();
        _ = host.RunAsync();

        var clientTransport = new StreamClientTransport(
            clientToServer.Writer.AsStream(),
            serverToClient.Reader.AsStream());

        await using var client =
            await McpClient.CreateAsync(
                clientTransport);

        var result = await client.CallToolAsync(
            "celsius_to_fahrenheit",
            new Dictionary<string, object?>
            {
                ["celsius"] = 100.0
            });

        var text = result.Content
            .OfType<TextContentBlock>()
            .First().Text;

        Assert.AreEqual("100C = 212.0F", text);

        await host.StopAsync();
    }

    [TestMethod]
    public async Task AllToolsAreDiscovered()
    {
        var clientToServer = new Pipe();
        var serverToClient = new Pipe();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(
                clientToServer.Reader.AsStream(),
                serverToClient.Writer.AsStream())
            .WithToolsFromAssembly();

        var host = builder.Build();
        _ = host.RunAsync();

        var clientTransport = new StreamClientTransport(
            clientToServer.Writer.AsStream(),
            serverToClient.Reader.AsStream());

        await using var client =
            await McpClient.CreateAsync(
                clientTransport);

        var tools = await client.ListToolsAsync();

        Assert.IsTrue(
            tools.Any(
                t => t.Name
                    == "celsius_to_fahrenheit"),
            "Should find celsius_to_fahrenheit");
        Assert.IsTrue(
            tools.Any(
                t => t.Name
                    == "fahrenheit_to_celsius"),
            "Should find fahrenheit_to_celsius");

        await host.StopAsync();
    }
}
