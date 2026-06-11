using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace PromptServer.Prompts;

[McpServerPromptType]
public static class CodeReviewPrompt
{
    // Returning a ChatMessage[] lets you build a multi-turn
    // conversation template. Each message maps to one turn
    // in the conversation the client will send to the AI model
    [McpServerPrompt]
    [Description(
        "Generates a code review prompt with "
        + "configurable language and style")]
    public static ChatMessage[] ReviewCode(
        [Description(
            "The programming language, "
            + "e.g. csharp, python, javascript")]
        string language,
        [Description(
            "Review style: thorough, quick, "
            + "or security-focused")]
        string style,
        [Description("The code to review")]
        string code)
    {
        // ChatRole.System sets the AI model's behavior and
        // persona — it is processed before user messages and
        // shapes how the model responds to everything that follows
        var systemMessage = new ChatMessage(
            ChatRole.System,
            $"You are an expert {language} code "
            + "reviewer. Perform a "
            + $"{style} review. Focus on best "
            + "practices, potential bugs, "
            + "readability, and maintainability. "
            + "Provide specific suggestions "
            + "with code examples where helpful.");

        // ChatRole.User represents what the human is asking —
        // together with the system message, this creates a
        // ready-to-send conversation that the client passes
        // directly to the AI model
        var userMessage = new ChatMessage(
            ChatRole.User,
            "Please review the following "
            + $"{language} code:\n\n"
            + $"```{language}\n{code}\n```");

        return [systemMessage, userMessage];
    }
}
