using CosmicPizzaServer.Models;

namespace CosmicPizzaServer.Services;

public interface IPizzaMenuService
{
    IReadOnlyList<Pizza> GetFullMenu();
    Pizza? GetPizza(string name);
    IReadOnlyList<Pizza> GetDailySpecials();
}
