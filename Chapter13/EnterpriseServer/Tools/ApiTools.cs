using System.ComponentModel;
using ModelContextProtocol.Server;

namespace EnterpriseServer.Tools;

[McpServerToolType]
public static class ApiTools
{
    [McpServerTool]
    [Description("Look up the current weather for a city using an external weather API.")]
    public static async Task<string> GetWeather(
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        [Description("City name to get weather for")] string city)
    {
        var logger = loggerFactory.CreateLogger("ApiTools");
        var client = httpClientFactory.CreateClient("WeatherApi");

        try
        {
            logger.LogInformation("Fetching weather for {City}", city);

            var response = await client.GetAsync($"/weather?city=" +
                Uri.EscapeDataString(city));

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return $"Weather data for {city}: {json}";
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Weather API call failed for {City}", city);

            return "Unable to fetch weather. The service may be "
                + "temporarily unavailable.";
        }
    }
}
