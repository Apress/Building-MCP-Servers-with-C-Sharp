namespace CosmicPizzaServer.Models;

public record Pizza(
    string Name,
    string Description,
    decimal Price,
    string Planet,
    string[] DefaultToppings);
