using CosmicPizzaServer.Models;

namespace CosmicPizzaServer.Services;

public class PizzaMenuService : IPizzaMenuService
{
    private readonly List<Pizza> _menu =
    [
        new Pizza(
            "The Mars Margherita",
            "A classic margherita with Martian red sauce, space-aged mozzarella, and "
            + "fresh basil grown in zero gravity.", 12.99m, "Mars",
            ["Martian Red Sauce", "Space Mozzarella", "Zero-G Basil"]),

        new Pizza(
            "Saturn's Ring-ion Burger Pizza",
            "A ring-shaped pizza loaded with cosmic ground beef, caramelized onion rings, "
            + "and a tangy asteroid ketchup drizzle.", 15.99m, "Saturn",
            ["Cosmic Beef", "Onion Rings", "Asteroid Ketchup", "Cheddar Nebula"]),

        new Pizza(
            "Neptune's Deep Sea Supreme",
            "Topped with space shrimp harvested from Neptune's methane oceans, tentacle-style "
            + "calamari, and a swirl of blue cheese from the outer rim.", 18.49m, "Neptune",
            ["Space Shrimp", "Cosmic Calamari", "Blue Cheese Nebula", "Seaweed Stardust"]),

        new Pizza(
            "Jupiter's Gas Giant Garlic Bread Pizza",
            "An oversized garlic bread pizza so big it has its own gravitational pull. Loaded "
            + "with roasted garlic butter and three types of melted cheese.", 14.49m, "Jupiter",
            ["Roasted Garlic Butter", "Triple Cheese", "Herb Constellation"]),

        new Pizza(
            "Pluto's Dwarf Delight",
            "Small but mighty! A personal-sized pizza packed with bold flavors. Spicy pepperoni "
            + "from the Kuiper Belt and jalapenos that burn hotter than a supernova.", 9.99m, "Pluto",
            ["Kuiper Belt Pepperoni", "Supernova Jalapenos", "Dwarf Cheese"])
    ];

    public IReadOnlyList<Pizza> GetFullMenu() => _menu.AsReadOnly();

    public Pizza? GetPizza(string name) =>
        _menu.FirstOrDefault(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<Pizza> GetDailySpecials()
    {
        var dayIndex = (int)DateTime.UtcNow.DayOfWeek;
        
        return _menu
            .Skip(dayIndex % _menu.Count)
            .Take(2)
            .ToList()
            .AsReadOnly();
    }
}
