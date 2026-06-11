using System.ComponentModel;
using ModelContextProtocol.Server;

namespace ResourceServer.Resources;

[McpServerResourceType]
public static class PlanetResources
{
    // This dictionary acts as our in-memory data store —
    // in production you might query a database instead
    private static readonly
        Dictionary<string, PlanetInfo> Planets =
        new(StringComparer.OrdinalIgnoreCase)
    {
        ["mercury"] = new(
            "Mercury",
            "Smallest planet and closest to " +
            "the Sun.",
            4879, 0.39, 0),
        ["venus"] = new(
            "Venus",
            "Hottest planet with a thick, " +
            "toxic atmosphere.",
            12104, 0.72, 0),
        ["earth"] = new(
            "Earth",
            "Our home planet, the only known " +
            "world with life.",
            12756, 1.0, 1),
        ["mars"] = new(
            "Mars",
            "The Red Planet with the tallest " +
            "volcano in the solar system.",
            6792, 1.52, 2),
        ["jupiter"] = new(
            "Jupiter",
            "Largest planet with a Great " +
            "Red Spot storm.",
            142984, 5.20, 95),
        ["saturn"] = new(
            "Saturn",
            "Famous for its stunning ring " +
            "system.",
            120536, 9.57, 146),
        ["uranus"] = new(
            "Uranus",
            "An ice giant that rotates on " +
            "its side.",
            51118, 19.17, 28),
        ["neptune"] = new(
            "Neptune",
            "The windiest planet in the " +
            "solar system.",
            49528, 30.07, 16)
    };

    // The {planetName} in the URI template makes this a
    // parameterized resource — the SDK extracts the value from
    // the URI and passes it as the method parameter. This lets
    // one method serve data for many different resources
    // (e.g., planets://info/mars, planets://info/jupiter).
    // MimeType "application/json" tells clients to parse the
    // response as structured JSON data
    [McpServerResource(
        UriTemplate =
            "planets://info/{planetName}",
        Name = "Planet Information",
        MimeType = "application/json")]
    [Description(
        "Returns detailed information " +
        "about a planet in our solar system.")]
    public static string GetPlanetInfo(
        string planetName)
    {
        if (!Planets.TryGetValue(
            planetName, out var planet))
        {
            return $$"""
                {
                  "error": "Unknown planet",
                  "requested": "{{planetName}}",
                  "available": [
                    "mercury", "venus", "earth",
                    "mars", "jupiter", "saturn",
                    "uranus", "neptune"
                  ]
                }
                """;
        }

        return $$"""
            {
              "name": "{{planet.Name}}",
              "description": "{{planet.Description}}",
              "diameterKm": {{planet.DiameterKm}},
              "distanceFromSunAU": {{planet.DistanceAU}},
              "knownMoons": {{planet.Moons}}
            }
            """;
    }

    private record PlanetInfo(
        string Name,
        string Description,
        int DiameterKm,
        double DistanceAU,
        int Moons);
}
