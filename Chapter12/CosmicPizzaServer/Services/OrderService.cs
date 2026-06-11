using System.Collections.Concurrent;
using CosmicPizzaServer.Models;

namespace CosmicPizzaServer.Services;

public class OrderService : IOrderService
{
    private readonly IPizzaMenuService _menuService;
    private readonly ConcurrentDictionary<string, Order> _orders = new();

    private static readonly Dictionary<string, double> PlanetDistances = new()
    {
        ["Mercury"] = 0.39,
        ["Venus"] = 0.72,
        ["Earth"] = 1.0,
        ["Mars"] = 1.52,
        ["Jupiter"] = 5.20,
        ["Saturn"] = 9.58,
        ["Uranus"] = 19.22,
        ["Neptune"] = 30.05,
        ["Pluto"] = 39.48
    };

    public OrderService(IPizzaMenuService menuService)
    {
        _menuService = menuService;
    }

    public Order PlaceOrder(
        string pizzaName,
        string size,
        string[] extraToppings,
        string deliveryPlanet)
    {
        var pizza = _menuService.GetPizza(pizzaName)
            ?? throw new ArgumentException($"Pizza '{pizzaName}' not found on menu");

        var sizeMultiplier = size.ToLower() switch
        {
            "small" or "dwarf" => 0.8m,
            "medium" or "standard" => 1.0m,
            "large" or "giant" => 1.3m,
            "cosmic" => 1.8m,
            _ => 1.0m
        };

        var toppingCost = extraToppings.Length * 1.50m;
        var totalPrice = (pizza.Price * sizeMultiplier) + toppingCost;

        var distance = PlanetDistances.GetValueOrDefault(deliveryPlanet, 10.0);

        var deliveryTime =
            $"{distance:F2} light-years " +
            $"({distance * 15:F0} galactic minutes)";

        var order = new Order(
            OrderId: GenerateOrderId(),
            PizzaName: pizza.Name,
            Size: size,
            ExtraToppings: extraToppings,
            DeliveryPlanet: deliveryPlanet,
            TotalPrice: totalPrice,
            EstimatedDelivery: deliveryTime,
            OrderedAt: DateTime.UtcNow);

        _orders[order.OrderId] = order;
        return order;
    }

    public Order? GetOrder(string orderId) =>
        _orders.GetValueOrDefault(orderId);

    public IReadOnlyList<Order> GetRecentOrders() =>
        _orders.Values
            .OrderByDescending(o => o.OrderedAt)
            .Take(10)
            .ToList()
            .AsReadOnly();

    private static string GenerateOrderId() =>
        $"COSMIC-{Guid.NewGuid().ToString()[..8].ToUpper()}";
}
