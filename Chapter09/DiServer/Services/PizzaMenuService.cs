namespace DiServer.Services;

public class PizzaMenuService : IPizzaMenuService
{
    private readonly Dictionary<string, (decimal Price, string Desc)>
        _menu = new()
    {
        ["Margherita"] = (8.99m,
            "Classic tomato sauce and mozzarella"),
        ["Pepperoni"] = (10.99m,
            "Loaded with spicy pepperoni slices"),
        ["Hawaiian"] = (11.49m,
            "Ham and pineapple, the debate continues"),
        ["Quattro Formaggi"] = (12.99m,
            "Four cheese blend of deliciousness"),
        ["Diavola"] = (11.99m,
            "Spicy salami with chili flakes")
    };

    public IReadOnlyList<string> GetAvailablePizzas() =>
        _menu.Keys.ToList().AsReadOnly();

    public decimal GetPrice(string pizzaName) =>
        _menu.TryGetValue(pizzaName, out var item)
            ? item.Price
            : throw new KeyNotFoundException(
                $"Pizza '{pizzaName}' not found");

    public string GetDescription(string pizzaName) =>
        _menu.TryGetValue(pizzaName, out var item)
            ? item.Desc
            : throw new KeyNotFoundException(
                $"Pizza '{pizzaName}' not found");
}
