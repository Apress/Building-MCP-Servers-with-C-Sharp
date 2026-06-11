namespace DiServer.Services;

public interface IPizzaMenuService
{
    IReadOnlyList<string> GetAvailablePizzas();
    decimal GetPrice(string pizzaName);
    string GetDescription(string pizzaName);
}
