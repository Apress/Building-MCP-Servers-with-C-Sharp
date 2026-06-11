using System.ComponentModel;
using Microsoft.Extensions.Caching.Memory;
using ModelContextProtocol.Server;

namespace NuGetToolDeployment.Tools;

[McpServerToolType]
public static class WeatherTools
{
    [McpServerTool]
    [Description(
        "Gets the current weather " +
        "for a city.")]
    public static string GetWeather(
        IMemoryCache cache,
        [Description("The city name")]
        string city)
    {
        var cacheKey =
            $"weather_{city.ToLowerInvariant()}";

        return cache.GetOrCreate(
            cacheKey, entry =>
        {
            entry
                .AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(5);

            var temperature =
                Random.Shared.Next(-10, 40);
            var conditions = new[]
            {
                "Sunny", "Cloudy", "Rainy",
                "Snowy", "Windy", "Foggy"
            };
            var condition = conditions[
                Random.Shared
                    .Next(conditions.Length)];

            return
                $"Weather in {city}: " +
                $"{temperature} C, " +
                $"{condition}";
        }) ?? "Weather data unavailable.";
    }

    [McpServerTool]
    [Description(
        "Gets a five day weather forecast.")]
    public static string GetForecast(
        [Description("The city name")]
        string city)
    {
        var forecasts = new List<string>();
        var conditions = new[]
        {
            "Sunny", "Cloudy", "Rainy",
            "Snowy", "Windy", "Foggy"
        };

        for (int i = 1; i <= 5; i++)
        {
            var date =
                DateTime.Now.AddDays(i);
            var temp =
                Random.Shared.Next(-10, 40);
            var condition = conditions[
                Random.Shared
                    .Next(conditions.Length)];
            forecasts.Add(
                $"{date:MMM dd}: " +
                $"{temp} C, {condition}");
        }

        return
            $"5-day forecast for {city}:\n" +
            string.Join("\n", forecasts);
    }
}
