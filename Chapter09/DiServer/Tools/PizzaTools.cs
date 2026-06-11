using DiServer.Models;
using DiServer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace DiServer.Tools;

[McpServerToolType]
public class PizzaTools
{
    [McpServerTool, Description(
        "Gets the full pizza menu with prices.")]
    public static string GetMenu(
        IPizzaMenuService menuService,
        IOptions<PizzaServerOptions> options,
        ILogger<PizzaTools> logger)
    {
        var config = options.Value;
        logger.LogInformation(
            "Menu requested for {Restaurant}",
            config.RestaurantName);

        var pizzas = menuService.GetAvailablePizzas();
        var lines = new List<string>
        {
            $"Welcome to {config.RestaurantName}!",
            ""
        };

        foreach (var pizza in pizzas)
        {
            var price = menuService.GetPrice(pizza);
            var desc = menuService.GetDescription(pizza);
            lines.Add(
                $"- {pizza}: {price} {config.Currency}");
            lines.Add($"  {desc}");
        }

        return string.Join("\n", lines);
    }

    [McpServerTool, Description(
        "Gets the price of a specific pizza.")]
    public static string GetPizzaPrice(
        [Description("Name of the pizza")]
        string pizzaName,
        IPizzaMenuService menuService,
        IOptions<PizzaServerOptions> options,
        ILogger<PizzaTools> logger)
    {
        var config = options.Value;
        logger.LogDebug(
            "Price lookup for {Pizza}", pizzaName);

        try
        {
            var price = menuService.GetPrice(pizzaName);
            return $"{pizzaName}: {price} " +
                   $"{config.Currency}";
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning(
                "Pizza not found: {Pizza}", pizzaName);
            return $"Sorry, '{pizzaName}' is not " +
                   "on our menu. Try GetMenu first!";
        }
    }
}
