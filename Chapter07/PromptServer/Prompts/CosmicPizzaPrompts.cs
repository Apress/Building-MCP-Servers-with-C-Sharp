using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace PromptServer.Prompts;

// A single [McpServerPromptType] class can contain multiple
// prompt methods — each becomes a separately listed prompt
// that clients can discover and invoke independently
[McpServerPromptType]
public static class CosmicPizzaPrompts
{
    [McpServerPrompt]
    [Description(
        "Recommends a space-themed pizza "
        + "based on your preferences")]
    public static ChatMessage[] PizzaRecommendation(
        [Description(
            "Preferred crust type: "
            + "thin, thick, or stuffed")]
        string crust,
        [Description(
            "Flavor preference: "
            + "savory, spicy, or sweet")]
        string flavor)
    {
        var system = new ChatMessage(
            ChatRole.System,
            "You are a cosmic pizza chef on a "
            + "space station orbiting Jupiter. "
            + "You specialize in creating "
            + "space-themed pizzas with creative "
            + "names and out-of-this-world "
            + "ingredients. Each pizza should "
            + "have a fun backstory about why "
            + "it was invented in space.");

        var user = new ChatMessage(
            ChatRole.User,
            $"I would like a {crust} crust pizza "
            + $"with a {flavor} flavor profile. "
            + "Please recommend a space-themed "
            + "pizza with a creative cosmic name, "
            + "list the toppings, and explain why "
            + "it is perfect for eating in "
            + "zero gravity.");

        return [system, user];
    }

    [McpServerPrompt]
    [Description(
        "Generates a space trivia quiz with "
        + "configurable difficulty and length")]
    public static ChatMessage[] SpaceTriviaQuiz(
        [Description(
            "Difficulty level: "
            + "easy, medium, or hard")]
        string difficulty,
        [Description(
            "Number of questions to generate "
            + "(between 1 and 10)")]
        int count)
    {
        // Prompt methods can include validation logic — the server
        // processes parameters before building the messages
        var clampedCount = Math.Clamp(count, 1, 10);

        var system = new ChatMessage(
            ChatRole.System,
            "You are an enthusiastic and fun "
            + "space trivia host. Generate "
            + "multiple-choice questions about "
            + "space, planets, stars, galaxies, "
            + "and space exploration history. "
            + "Include the correct answer after "
            + "each question with a brief "
            + "explanation of why it is correct.");

        var user = new ChatMessage(
            ChatRole.User,
            $"Generate {clampedCount} {difficulty} "
            + "space trivia questions. Format each "
            + "as a multiple-choice question with "
            + "four options labeled A, B, C, and D. "
            + "After each question, reveal the "
            + "correct answer.");

        return [system, user];
    }
}
