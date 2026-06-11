namespace CosmicPizzaServer.Models;

public class CosmicPizzaOptions
{
    public const string SectionName = "CosmicPizza";

    public string RestaurantName { get; set; }
        = "Cosmic Pizza - Intergalactic Pizzeria";

    public string Currency { get; set; } = "GalactiCoins";

    public int MaxToppingsPerPizza { get; set; } = 6;

    public int DeliverySpeedLightYearsPerHour { get; set; } = 42;
}
