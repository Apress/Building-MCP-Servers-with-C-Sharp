using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

// Path to the MCP server project
var serverPath = args.Length > 0
    ? args[0]
    : "../../Chapter09/DiServer";

Console.WriteLine("=== MCP Client Application ===");
Console.WriteLine(
    $"Connecting to server: {serverPath}");
Console.WriteLine();

// Create the MCP client using stdio transport
await using var client =
    await McpClient.CreateAsync(
        new StdioClientTransport(new()
        {
            Command = "dotnet",
            Arguments = ["run", "--project",
                serverPath],
            Name = "Cosmic Pizza Server",
        }));

Console.WriteLine(
    $"Connected to: {client.ServerInfo.Name} " +
    $"v{client.ServerInfo.Version}");
Console.WriteLine();

// Main interaction loop
while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("  1. List tools");
    Console.WriteLine("  2. Call a tool");
    Console.WriteLine("  3. List resources");
    Console.WriteLine("  4. List prompts");
    Console.WriteLine("  5. Exit");
    Console.Write("> ");

    var choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            await ListTools(client);
            break;
        case "2":
            await CallTool(client);
            break;
        case "3":
            await ListResources(client);
            break;
        case "4":
            await ListPrompts(client);
            break;
        case "5":
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }

    Console.WriteLine();
}

static async Task ListTools(McpClient client)
{
    var tools = await client.ListToolsAsync();
    Console.WriteLine($"Found {tools.Count} tool(s):");

    foreach (var tool in tools)
    {
        Console.WriteLine($"Tool: {tool.Name}");

        var schema = tool.JsonSchema;
        
        if (schema.TryGetProperty("properties", out var props))
        {
            Console.WriteLine("  Parameters:");
            foreach (var prop in props.EnumerateObject())
            {
                Console.WriteLine($"    {prop.Name}: {prop.Value}");
            }
        }
    }
}

static async Task CallTool(McpClient client)
{
    var tools = await client.ListToolsAsync();

    Console.WriteLine("Available tools:");
    for (var i = 0; i < tools.Count; i++)
    {
        Console.WriteLine(
            $"  {i + 1}. {tools[i].Name}");
    }

    Console.Write("Select a tool number: ");
    if (!int.TryParse(Console.ReadLine(), out var num)
        || num < 1 || num > tools.Count)
    {
        Console.WriteLine("Invalid selection.");
        return;
    }

    var selected = tools[num - 1];
    Console.WriteLine(
        $"Calling tool: {selected.Name}");

    // Collect arguments if the tool has parameters
    var arguments =
        new Dictionary<string, object?>();

    var schema = selected.JsonSchema;
    if (schema.TryGetProperty(
        "properties", out var props))
    {
        foreach (var prop in
            props.EnumerateObject())
        {
            Console.Write(
                $"  {prop.Name}: ");
            var value = Console.ReadLine();
            if (!string.IsNullOrEmpty(value))
            {
                arguments[prop.Name] = value;
            }
        }
    }

    try
    {
        var result =
            await client.CallToolAsync(
                selected.Name,
                arguments.Count > 0
                    ? arguments : null);

        Console.WriteLine("Result:");
        foreach (var content in result.Content)
        {
            if (content
                is TextContentBlock text)
            {
                Console.WriteLine(
                    $"  {text.Text}");
            }
        }
    }
    catch (McpException ex)
    {
        Console.WriteLine(
            $"Error: {ex.Message}");
    }
}

static async Task ListResources(McpClient client)
{
    try
    {
        var resources =
            await client.ListResourcesAsync();
        Console.WriteLine(
            $"Found {resources.Count} " +
            "resource(s):");

        foreach (var resource in resources)
        {
            Console.WriteLine(
                $"  - {resource.Name}");
            Console.WriteLine(
                $"    URI: {resource.Uri}");
        }
    }
    catch (McpException ex)
    {
        Console.WriteLine(
            $"Resources not supported: " +
            $"{ex.Message}");
    }
}

static async Task ListPrompts(McpClient client)
{
    try
    {
        var prompts =
            await client.ListPromptsAsync();
        Console.WriteLine(
            $"Found {prompts.Count} prompt(s):");

        foreach (var prompt in prompts)
        {
            Console.WriteLine(
                $"  - {prompt.Name}");
            Console.WriteLine(
                $"    {prompt.Description}");
        }
    }
    catch (McpException ex)
    {
        Console.WriteLine(
            $"Prompts not supported: " +
            $"{ex.Message}");
    }
}
