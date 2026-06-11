using CosmicPizzaServer.Models;

namespace CosmicPizzaServer.Services;

public interface IOrderService
{
    Order PlaceOrder(
        string pizzaName,
        string size,
        string[] extraToppings,
        string deliveryPlanet);

    Order? GetOrder(string orderId);
    IReadOnlyList<Order> GetRecentOrders();
}
