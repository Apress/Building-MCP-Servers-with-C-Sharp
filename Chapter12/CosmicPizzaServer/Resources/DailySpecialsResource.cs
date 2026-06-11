using System.ComponentModel;
using System.Text;
using CosmicPizzaServer.Models;
using CosmicPizzaServer.Services;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Resources;

[McpServerResourceType]
public class DailySpecialsResource
{
    [McpServerResource(
        UriTemplate = "cosmic-pizza://daily-specials",
        Name = "Daily Specials",
        MimeType = "text/plain")]
    [Description("Today's special cosmic pizza deals.")]
    public static string GetDailySpecials(
        IPizzaMenuService menuService,
        IOptions<CosmicPizzaOptions> options)
    {
        var config = options.Value;
        var specials = menuService.GetDailySpecials();
        var dayName = DateTime.UtcNow.DayOfWeek.ToString();
        var sb = new StringBuilder();
        sb.AppendLine($"{dayName} Specials!");
        sb.AppendLine("========================");

        foreach (var pizza in specials)
        {
            var discountPrice = pizza.Price * 0.8m;
            sb.AppendLine($"  {pizza.Name}");
            sb.AppendLine($"  Was: {pizza.Price} {config.Currency}");
            sb.AppendLine($"  NOW: {discountPrice:F2} {config.Currency} (20% off!)");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
