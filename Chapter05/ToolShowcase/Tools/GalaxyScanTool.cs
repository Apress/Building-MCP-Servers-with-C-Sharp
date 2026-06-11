using System.ComponentModel;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

[McpServerToolType]
public static class GalaxyScanTool
{
    [McpServerTool(Name = "scan_galaxy")]
    [Description(
        "Scans a galaxy sector for anomalies. " +
        "Reports progress during the scan.")]
    public static async Task<string> ScanGalaxy(
        [Description("The galaxy sector to scan")]
        string sector = "Alpha-7",
        [Description("Scan depth (1-10)")]
        int depth = 5,
        IProgress<ProgressNotificationValue>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var anomalies = new List<string>();
        var scanSteps = new[]
        {
            "Calibrating sensors",
            "Scanning for electromagnetic signatures",
            "Analyzing gravitational anomalies",
            "Mapping stellar formations",
            "Detecting life signs",
            "Compiling scan results"
        };

        for (int i = 0; i < scanSteps.Length; i++)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            progress?.Report(
                new ProgressNotificationValue
                {
                    Progress = i + 1,
                    Total = scanSteps.Length,
                    Message = $"{scanSteps[i]}..."
                });

            await Task.Delay(
                500, cancellationToken);

            if (Random.Shared.NextDouble() > 0.5)
            {
                anomalies.Add(
                    $"Anomaly detected during: " +
                    $"{scanSteps[i]}");
            }
        }

        progress?.Report(
            new ProgressNotificationValue
            {
                Progress = scanSteps.Length,
                Total = scanSteps.Length,
                Message = "Scan complete!"
            });

        var result = anomalies.Count > 0
            ? string.Join("\n", anomalies)
            : "No anomalies detected.";

        return $"Sector {sector} scan complete " +
            $"(depth {depth}).\n" +
            $"Found {anomalies.Count} anomalies:\n" +
            result;
    }
}
