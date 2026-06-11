using System.ComponentModel;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace ToolShowcase.Tools;

[McpServerToolType]
public static class ErrorHandlingTools
{
    [McpServerTool(Name = "divide")]
    [Description("Divides two numbers safely.")]
    public static string Divide(
        [Description("The dividend")]
        double a,
        [Description("The divisor")]
        double b)
    {
        if (b == 0)
        {
            throw new McpException(
                "Cannot divide by zero. " +
                "Please provide a non-zero divisor.");
        }

        return $"{a} / {b} = {a / b}";
    }

    [McpServerTool(Name = "validate_email")]
    [Description("Validates an email address.")]
    public static CallToolResult ValidateEmail(
        [Description(
            "The email address to validate")]
        string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new CallToolResult
            {
                Content =
                [
                    new TextContentBlock
                    {
                        Text = "Email cannot " +
                            "be empty."
                    }
                ],
                IsError = true
            };
        }

        if (!email.Contains('@') ||
            !email.Contains('.'))
        {
            return new CallToolResult
            {
                Content =
                [
                    new TextContentBlock
                    {
                        Text = $"'{email}' is " +
                            "not valid."
                    }
                ],
                IsError = true
            };
        }

        return new CallToolResult
        {
            Content =
            [
                new TextContentBlock
                {
                    Text = $"'{email}' is valid."
                }
            ],
            IsError = false
        };
    }
}
