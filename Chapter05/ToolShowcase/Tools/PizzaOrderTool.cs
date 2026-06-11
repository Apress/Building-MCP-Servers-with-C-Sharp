using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

public enum PizzaSize
{
    Small,
    Medium,
    Large,
    ExtraLarge
}

public enum CrustType
{
    Thin,
    Regular,
    ThickCrust,
    Stuffed
}

[McpServerToolType]
public static class PizzaOrderTool
{
    [McpServerTool(Name = "order_pizza")]
    [Description(
        "Places a pizza order with full customization.")]
    public static string OrderPizza(
        [Description("The size of the pizza")]
        PizzaSize size,
        [Description("The type of crust")]
        CrustType crust = CrustType.Regular,
        [Description(
            "Comma-separated list of toppings")]
        string toppings = "cheese",
        [Description("Number of pizzas to order")]
        int quantity = 1,
        [Description("Add extra cheese")]
        bool extraCheese = false,
        [Description("Special delivery instructions")]
        string? deliveryNotes = null)
    {
        var toppingsList = toppings
            .Split(',')
            .Select(t => t.Trim())
            .ToList();

        var order = new
        {
            OrderId = Guid.NewGuid()
                .ToString()[..8],
            Size = size.ToString(),
            Crust = crust.ToString(),
            Toppings = toppingsList,
            Quantity = quantity,
            ExtraCheese = extraCheese,
            DeliveryNotes = deliveryNotes
                ?? "None",
            EstimatedDelivery = DateTime.Now
                .AddMinutes(30)
                .ToString("HH:mm")
        };

        var json = JsonSerializer.Serialize(
            order,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        return $"Order confirmed!\n{json}";
    }
}
