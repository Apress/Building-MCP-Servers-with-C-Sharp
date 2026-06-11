using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace PromptServer.Prompts;

[McpServerPromptType]
public static class SimplePrompts
{
    // A prompt method can return a single ChatMessage instead
    // of an array — this is the simplest form, useful when
    // you only need a user message without a system instruction
    [McpServerPrompt]
    [Description(
        "Generates a friendly greeting message")]
    public static ChatMessage Greeting(
        [Description("Name of the person to greet")]
        string name)
    {
        return new ChatMessage(
            ChatRole.User,
            $"Please write a warm and friendly "
            + $"greeting for {name}. Make it "
            + "enthusiastic and welcoming.");
    }
}
