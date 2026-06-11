using System.ComponentModel;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

[McpServerToolType]
public static class BasicTools
{
    [McpServerTool(Name = "greet")]
    [Description("Greets the user by name.")]
    public static string Greet(
        [Description("The name of the person to greet")]
        string name)
    {
        return $"Hello, {name}! Welcome to the " +
            "ToolShowcase MCP server.";
    }

    [McpServerTool(Name = "add_numbers")]
    [Description("Adds two numbers together.")]
    public static string AddNumbers(
        [Description("The first number")]
        double a,
        [Description("The second number")]
        double b)
    {
        var result = a + b;
        return $"The sum of {a} and {b} is {result}.";
    }
}
