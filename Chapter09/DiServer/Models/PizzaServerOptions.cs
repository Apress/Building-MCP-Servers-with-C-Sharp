namespace DiServer.Models;

public class PizzaServerOptions
{
    public const string SectionName = "PizzaServer";

    public string RestaurantName { get; set; } = "MCP Pizzeria";
    public string Currency { get; set; } = "USD";
    public int MaxToppingsPerPizza { get; set; } = 3;
}
