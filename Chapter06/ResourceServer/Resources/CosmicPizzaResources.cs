using System.ComponentModel;
using ModelContextProtocol.Server;

namespace ResourceServer.Resources;

// [McpServerResourceType] marks this class as a container for
// MCP resource methods, similar to how [McpServerToolType]
// works for tools
[McpServerResourceType]
public static class CosmicPizzaResources
{
    // The UriTemplate defines the unique address clients use
    // to read this resource. A static URI like "cosmic-pizza://menu"
    // (with no {parameters}) always returns the same resource.
    // MimeType tells clients how to interpret the response —
    // "text/plain" means the client should render it as plain text
    [McpServerResource(
        UriTemplate = "cosmic-pizza://menu",
        Name = "Full Menu",
        MimeType = "text/plain")]
    [Description(
        "The complete Cosmic Pizza menu " +
        "with prices.")]
    public static string GetMenu()
    {
        return """
            Cosmic Pizza Menu
            =================

            Nebula Margherita    - $12.99
            Black Hole BBQ       - $15.99
            Asteroid Supreme     - $14.99
            Milky Way White      - $13.99
            Supernova Special    - $17.99

            Sides
            -----
            Galactic Garlic Bread  - $5.99
            Comet Cheese Sticks    - $7.99
            Orbit Onion Rings      - $6.99

            Drinks
            ------
            Starlight Soda         - $2.99
            Lunar Lemonade         - $3.49
            Rocket Fuel Espresso   - $4.99
            """;
    }

    [McpServerResource(
        UriTemplate = "cosmic-pizza://hours",
        Name = "Opening Hours",
        MimeType = "text/plain")]
    [Description(
        "Cosmic Pizza opening hours " +
        "for all locations.")]
    public static string GetOpeningHours()
    {
        return """
            Cosmic Pizza Opening Hours
            ==========================

            Monday - Thursday:  11:00 AM - 10:00 PM
            Friday - Saturday:  11:00 AM - 12:00 AM
            Sunday:             12:00 PM -  9:00 PM

            Holiday hours may vary.
            Order online at cosmicpizza.example.com
            """;
    }
}
