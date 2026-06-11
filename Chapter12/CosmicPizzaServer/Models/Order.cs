namespace CosmicPizzaServer.Models;

public record Order(
    string OrderId,
    string PizzaName,
    string Size,
    string[] ExtraToppings,
    string DeliveryPlanet,
    decimal TotalPrice,
    string EstimatedDelivery,
    DateTime OrderedAt);
