using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class ValidationTools
{
    [McpServerTool]
    [Description("Gets info about a page")]
    public static string GetPageInfo(int pageNumber)
    {
        if (pageNumber < 1)
            return "Error: Page number must be positive.";

        return $"Page {pageNumber} info.";
    }
}
