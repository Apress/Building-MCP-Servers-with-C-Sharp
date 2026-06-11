using System.ComponentModel;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Tools;

[McpServerToolType]
public static class SpaceFactsTool
{
    private static readonly string[] SpaceFacts =
    [
        "A day on Venus is longer than a year on " +
        "Venus. Talk about a long Monday!",

        "Neutron stars are so dense that a " +
        "teaspoon would weigh about 6 billion tons.",

        "There are more stars in the universe than " +
        "grains of sand on all of Earth's beaches.",

        "The footprints on the Moon will last for " +
        "100 million years since there is no wind.",

        "Space is completely silent because there " +
        "is no atmosphere for sound to travel through.",

        "Saturn's density is low enough that it " +
        "would float in water, if you had a big " +
        "enough bathtub.",

        "One million Earths could fit inside the " +
        "Sun. That is a lot of pizza deliveries.",

        "The International Space Station travels " +
        "at about 28,000 km/h, completing an orbit " +
        "every 90 minutes.",

        "There is a planet made of diamonds called " +
        "55 Cancri e. Our delivery team refuses to " +
        "go there due to parking fees.",

        "The Milky Way galaxy is on a collision " +
        "course with Andromeda, but not for another " +
        "4.5 billion years. Plenty of time for pizza.",

        "Olympus Mons on Mars is the tallest " +
        "mountain in our solar system at about " +
        "22 km high. We deliver there too.",

        "A year on Mercury is just 88 Earth days. " +
        "Imagine how many birthday pizzas you would " +
        "need!",

        "The Great Red Spot on Jupiter is a storm " +
        "that has been raging for at least 350 " +
        "years. Perfect weather for garlic bread.",

        "If you could drive a car at highway speed " +
        "to Pluto, it would take about 6,000 years.",

        "Astronauts grow up to 2 inches taller in " +
        "space due to the lack of gravity compressing" +
        " their spines."
    ];

    [McpServerTool(Name = "get_space_fact")]
    [Description(
        "Returns a random fascinating space fact. " +
        "Educational and entertaining!")]
    public static string GetSpaceFact()
    {
        var index = Random.Shared.Next(SpaceFacts.Length);

        return $"Space Fact: {SpaceFacts[index]}";
    }

    [McpServerTool(Name = "get_space_facts")]
    [Description("Returns multiple random space facts.")]
    public static string GetMultipleSpaceFacts(
        [Description("Number of facts to return (1-5)")] int count = 3)
    {
        count = Math.Clamp(count, 1, 5);
        
        var facts = SpaceFacts
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .Select((f, i) => $"{i + 1}. {f}");

        return "Random Space Facts:\n" +
            string.Join("\n\n", facts);
    }
}
