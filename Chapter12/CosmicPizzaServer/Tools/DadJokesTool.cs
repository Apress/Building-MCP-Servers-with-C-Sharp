using System.ComponentModel;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Tools;

[McpServerToolType]
public static class DadJokesTool
{
    private static readonly string[] SpaceDadJokes =
    [
        "Why did the sun go to school? " +
        "To get a little brighter!",

        "How do you organize a space " +
        "party? You planet!",

        "Why did the cow go to outer " +
        "space? To see the Milky Way!",

        "What is an astronaut's favorite key on the keyboard? " +
        "The space bar!",

        "How does the solar system hold up its pants? " +
        "With an asteroid belt!",

        "Why was the restaurant on the " +
        "moon so bad? No atmosphere!"
    ];

    [McpServerTool(Name = "get_space_dad_joke")]
    [Description("Returns a random space-themed dad " +
        "joke. Guaranteed to make you groan.")]
    public static string GetDadJoke()
    {
        var index = Random.Shared
            .Next(SpaceDadJokes.Length);

        return SpaceDadJokes[index];
    }

    [McpServerTool(Name = "get_space_dad_jokes")]
    [Description("Returns multiple space-themed dad jokes.")]
    public static string GetMultipleDadJokes(
        [Description("Number of jokes (1-5)")]
        int count = 3)
    {
        count = Math.Clamp(count, 1, 5);

        var jokes = SpaceDadJokes
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .ToArray();

        return "Space Dad Jokes:\n" +
            string.Join("\n\n", jokes);
    }
}
