using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace PromptServer.Prompts;

[McpServerPromptType]
public static class WriteTestsPrompt
{
    [McpServerPrompt]
    [Description(
        "Sets up a unit test writing session")]
    public static ChatMessage[] WriteTests(
        [Description(
        "The class or method to test")]
        string targetCode,
        [Description(
        "Testing framework: xunit, nunit, "
        + "or mstest")]
        string framework)
    {
        var system = new ChatMessage(
            ChatRole.System,
            "You are an expert at writing unit "
            + $"tests using {framework}. Write "
            + "thorough tests that cover edge "
            + "cases, error conditions, and "
            + "happy paths. Use the Arrange-Act-"
            + "Assert pattern.");
        var user = new ChatMessage(
            ChatRole.User,
            "Please write unit "
            + "tests for the following code:\n\n"
            + $"```csharp\n{targetCode}\n```");
        return [system, user];
    }
}
