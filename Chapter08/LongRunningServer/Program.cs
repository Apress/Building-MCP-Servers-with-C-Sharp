using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new Implementation
        {
            Name = "LongRunningServer",
            Version = "1.0.0"
        };
    })
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for most HTTP servers.
        // Each request is independent — no session tracking needed.
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

var app = builder.Build();
app.MapMcp();
app.Run();

// A tool that simulates a multi-step text analysis pipeline.
// Each step takes a few seconds and reports progress back to the
// client, so you can see updates arrive in real time.
[McpServerToolType]
public static class TextAnalysisTools
{
    [McpServerTool(Name = "analyze_text"),
     Description("Analyzes text through a multi-step pipeline: " +
        "word count, readability, sentiment, and summary. " +
        "Reports progress after each step.")]
    public static async Task<string> AnalyzeText(
        [Description("The text to analyze")]
        string text,
        McpServer server,
        RequestContext<CallToolRequestParams> context,
        CancellationToken cancellationToken)
    {
        var progressToken = context.Params?.ProgressToken;
        var steps = new[]
        {
            "Counting words and characters",
            "Calculating readability score",
            "Analyzing sentiment",
            "Generating summary"
        };

        var results = new List<string>();

        for (int i = 0; i < steps.Length; i++)
        {
            // Report progress if the client requested it
            if (progressToken is not null)
            {
                await server.SendNotificationAsync(
                    "notifications/progress",
                    new ProgressNotificationParams
                    {
                        ProgressToken = progressToken.Value,
                        Progress = new ProgressNotificationValue
                        {
                            Progress = i,
                            Total = steps.Length,
                            Message = steps[i],
                        },
                    });
            }

            // Simulate each analysis step taking some time
            await Task.Delay(TimeSpan.FromSeconds(2),
                cancellationToken);

            // Perform the actual analysis step
            results.Add(steps[i] switch
            {
                "Counting words and characters" =>
                    $"Words: {text.Split(' ',
                        StringSplitOptions.RemoveEmptyEntries)
                        .Length}, " +
                    $"Characters: {text.Length}",

                "Calculating readability score" =>
                    CalculateReadability(text),

                "Analyzing sentiment" =>
                    AnalyzeSentiment(text),

                "Generating summary" =>
                    text.Length > 100
                        ? $"Summary: {text[..97]}..."
                        : $"Summary: {text}",

                _ => "Unknown step"
            });
        }

        // Send final progress notification
        if (progressToken is not null)
        {
            await server.SendNotificationAsync(
                "notifications/progress",
                new ProgressNotificationParams
                {
                    ProgressToken = progressToken.Value,
                    Progress = new ProgressNotificationValue
                    {
                        Progress = steps.Length,
                        Total = steps.Length,
                        Message = "Analysis complete!",
                    },
                });
        }

        return string.Join("\n", results);
    }

    private static string CalculateReadability(string text)
    {
        var words = text.Split(' ',
            StringSplitOptions.RemoveEmptyEntries);
        var sentences = text.Split(['.', '!', '?'],
            StringSplitOptions.RemoveEmptyEntries);
        var avgWordsPerSentence = sentences.Length > 0
            ? (double)words.Length / sentences.Length
            : words.Length;

        // Simplified readability: shorter sentences = easier
        var level = avgWordsPerSentence switch
        {
            < 10 => "Easy",
            < 20 => "Moderate",
            _ => "Complex"
        };

        return $"Readability: {level} " +
            $"(avg {avgWordsPerSentence:F1} words/sentence)";
    }

    private static string AnalyzeSentiment(string text)
    {
        // Very simplified sentiment analysis based on keyword
        // matching. A real implementation would use an NLP
        // library or call an AI model.
        var lower = text.ToLowerInvariant();
        var positive = new[] { "good", "great", "excellent",
            "happy", "love", "wonderful", "fantastic",
            "amazing", "brilliant" };
        var negative = new[] { "bad", "terrible", "awful",
            "hate", "horrible", "worst", "poor",
            "disappointing", "boring" };

        var posCount = positive.Count(w =>
            lower.Contains(w));
        var negCount = negative.Count(w =>
            lower.Contains(w));

        var sentiment = (posCount - negCount) switch
        {
            > 0 => "Positive",
            < 0 => "Negative",
            _ => "Neutral"
        };

        return $"Sentiment: {sentiment} " +
            $"(+{posCount}/-{negCount} signals)";
    }
}
