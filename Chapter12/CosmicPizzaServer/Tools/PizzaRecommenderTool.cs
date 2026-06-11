using System.ComponentModel;
using CosmicPizzaServer.Models;
using CosmicPizzaServer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Tools;

[McpServerToolType]
public class PizzaRecommenderTool
{
    private static readonly Dictionary<string, string> ZodiacPizzas = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Aries"] = "Pluto's Dwarf Delight",
        ["Taurus"] = "Jupiter's Gas Giant Garlic Bread Pizza",
        ["Gemini"] = "Saturn's Ring-ion Burger Pizza",
        ["Cancer"] = "Neptune's Deep Sea Supreme",
        ["Leo"] = "The Mars Margherita",
        ["Virgo"] = "Pluto's Dwarf Delight",
        ["Libra"] = "Saturn's Ring-ion Burger Pizza",
        ["Scorpio"] = "Pluto's Dwarf Delight",
        ["Sagittarius"] = "Jupiter's Gas Giant Garlic Bread Pizza",
        ["Capricorn"] = "The Mars Margherita",
        ["Aquarius"] = "Neptune's Deep Sea Supreme",
        ["Pisces"] = "Neptune's Deep Sea Supreme"
    };

    [McpServerTool(Name = "recommend_pizza")]
    [Description("Recommends a cosmic pizza based on your zodiac sign.")]
    public static string RecommendPizza(
        [Description("Your zodiac sign, e.g. Aries")] string zodiacSign,
        IPizzaMenuService menuService,
        IOptions<CosmicPizzaOptions> options,
        ILogger<PizzaRecommenderTool> logger)
    {
        logger.LogInformation("Recommendation for {Sign}", zodiacSign);

        var config = options.Value;

        if (!ZodiacPizzas.TryGetValue(
            zodiacSign, out var pizzaName))
        {
            return $"'{zodiacSign}' is not a " +
                "recognized zodiac sign.";
        }

        var pizza = menuService.GetPizza(pizzaName);

        if (pizza is null)
            return "Pizza not found!";

        return $"Based on your sign " +
            $"({zodiacSign}):\n\n" +
            $"  {pizza.Name}\n" +
            $"  {pizza.Description}\n" +
            $"  Price: {pizza.Price} {config.Currency}";
    }
}
