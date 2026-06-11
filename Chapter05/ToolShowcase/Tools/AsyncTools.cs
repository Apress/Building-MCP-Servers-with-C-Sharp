using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

[McpServerToolType]
public static class AsyncTools
{
    [McpServerTool(
        Name = "fetch_random_fact",
        ReadOnly = true)]
    [Description(
        "Fetches a random fact from the internet.")]
    public static async Task<string> FetchRandomFact(
        [Description("The category of fact")]
        string category = "general",
        CancellationToken cancellationToken = default)
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);

        try
        {
            var response = await httpClient.GetAsync(
                "https://uselessfacts.jsph.pl" +
                "/api/v2/facts/random?language=en",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content
                .ReadAsStringAsync(cancellationToken);

            using var doc = JsonDocument.Parse(json);
            var fact = doc.RootElement
                .GetProperty("text")
                .GetString();

            return $"Random fact ({category}): {fact}";
        }
        catch (OperationCanceledException)
        {
            return "The request was cancelled.";
        }
        catch (HttpRequestException ex)
        {
            return $"Could not fetch fact: {ex.Message}";
        }
    }
}
