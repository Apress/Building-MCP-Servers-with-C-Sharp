using System.ComponentModel;
using System.Text;
using CosmicPizzaServer.Models;
using CosmicPizzaServer.Services;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Resources;

[McpServerResourceType]
public class MenuResource
{
    [McpServerResource(
        UriTemplate = "cosmic-pizza://menu",
        Name = "Cosmic Pizza Full Menu",
        MimeType = "text/plain")]
    [Description("The complete Cosmic Pizza menu.")]
    public static string GetFullMenu(
        IPizzaMenuService menuService,
        IOptions<CosmicPizzaOptions> options)
    {
        var config = options.Value;
        var sb = new StringBuilder();
        sb.AppendLine("==========================");
        sb.AppendLine("   COSMIC PIZZA MENU");
        sb.AppendLine("==========================");
        sb.AppendLine();

        foreach (var pizza in menuService.GetFullMenu())
        {
            sb.AppendLine($"  {pizza.Name}");
            sb.AppendLine($"  {pizza.Description}");
            sb.AppendLine($"  Price: {pizza.Price} {config.Currency}");
            sb.AppendLine($"  Origin: {pizza.Planet}");
            sb.AppendLine($"  Toppings: " +
                string.Join(", ", pizza.DefaultToppings));
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
