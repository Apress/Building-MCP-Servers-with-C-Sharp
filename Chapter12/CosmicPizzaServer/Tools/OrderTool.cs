using System.ComponentModel;
using CosmicPizzaServer.Models;
using CosmicPizzaServer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Tools;

[McpServerToolType]
public class OrderTool
{
    [McpServerTool(Name = "place_pizza_order")]
    [Description("Places a cosmic pizza order for delivery "
        + "to any planet in the solar system.")]
    public static string PlaceOrder(
        [Description("Name of the pizza to order")] string pizzaName,
        [Description("Size: small, medium, large, or cosmic")] string size,
        [Description("Planet for delivery, e.g. Mars, Earth")] string deliveryPlanet,
        [Description("Extra toppings as comma-separated list")] string extraToppings,
        IOrderService orderService,
        IOptions<CosmicPizzaOptions> options,
        ILogger<OrderTool> logger)
    {
        var config = options.Value;
        logger.LogInformation(
            "New order: {Pizza} ({Size}) to {Planet}",
            pizzaName, size, deliveryPlanet);

        var toppings = string.IsNullOrWhiteSpace(
            extraToppings)
            ? Array.Empty<string>()
            : extraToppings.Split(',', StringSplitOptions.TrimEntries);

        if (toppings.Length > config.MaxToppingsPerPizza)
        {
            return "Whoa there, space cowboy! " +
                $"Maximum {config.MaxToppingsPerPizza}" +
                " extra toppings allowed. Even our " +
                "cosmic ovens have limits!";
        }

        try
        {
            var order = orderService.PlaceOrder(
                pizzaName, size, toppings,
                deliveryPlanet);

            return FormatOrderConfirmation(
                order, config);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning("Order failed: {Error}", ex.Message);

            return $"Order failed: {ex.Message}. Use get_menu to see available pizzas!";
        }
    }

    [McpServerTool(Name = "get_order_status")]
    [Description("Checks the status of an existing order.")]
    public static string GetOrderStatus(
        [Description("The order ID to look up")] string orderId,
        IOrderService orderService,
        ILogger<OrderTool> logger)
    {
        logger.LogDebug("Status check for {OrderId}", orderId);

        var order = orderService.GetOrder(orderId);
        if (order is null)
        {
            return $"Order '{orderId}' not found. " +
                "It may have been lost in a wormhole!";
        }

        var elapsed = DateTime.UtcNow - order.OrderedAt;
        return $"Order {order.OrderId}:\n" +
            $"  Pizza: {order.PizzaName}\n" +
            $"  Destination: {order.DeliveryPlanet}\n" +
            $"  ETA: {order.EstimatedDelivery}\n" +
            $"  Time in transit: {elapsed.TotalMinutes:F0} Earth minutes";
    }

    private static string FormatOrderConfirmation(
        Order order, CosmicPizzaOptions config)
    {
        var toppingsText = order.ExtraToppings.Length > 0
            ? string.Join(", ", order.ExtraToppings)
            : "None";

        return
            "--- COSMIC PIZZA ORDER CONFIRMED ---\n" +
            $"Order ID: {order.OrderId}\n" +
            $"Pizza: {order.PizzaName}\n" +
            $"Size: {order.Size}\n" +
            $"Extra Toppings: {toppingsText}\n" +
            $"Delivery To: {order.DeliveryPlanet}\n" +
            $"Total: {order.TotalPrice:F2} " +
            $"{config.Currency}\n" +
            $"Estimated Delivery: " +
            $"{order.EstimatedDelivery}\n" +
            "------------------------------------\n" +
            "Your pizza is being prepared in our " +
            "zero-gravity kitchen!";
    }
}
