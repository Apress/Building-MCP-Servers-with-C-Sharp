using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace DiServer.Tools;

[McpServerToolType]
public class JokeTools
{
    [McpServerTool, Description(
        "Fetches a random joke from an external API.")]
    public static async Task<string> GetJoke(
        IHttpClientFactory httpClientFactory,
        ILogger<JokeTools> logger)
    {
        logger.LogInformation("Fetching a joke...");

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add(
            "Accept", "application/json");

        try
        {
            var response = await client.GetStringAsync(
                "https://official-joke-api.appspot.com"
                + "/random_joke");

            var joke = JsonDocument.Parse(response);
            var setup = joke.RootElement
                .GetProperty("setup").GetString();
            var punchline = joke.RootElement
                .GetProperty("punchline").GetString();

            logger.LogInformation("Joke fetched OK");
            return $"{setup}\n\n{punchline}";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch joke");
            return "Could not fetch a joke right now. "
                 + "Try again later!";
        }
    }
}
