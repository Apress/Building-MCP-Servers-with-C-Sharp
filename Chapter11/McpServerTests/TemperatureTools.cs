using System.Globalization;
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class TemperatureTools
{
    [McpServerTool]
    [Description("Converts Celsius to Fahrenheit")]
    public static string CelsiusToFahrenheit(
        double celsius)
    {
        var f = celsius * 9.0 / 5.0 + 32;
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0}C = {1:F1}F", celsius, f);
    }

    [McpServerTool]
    [Description("Converts Fahrenheit to Celsius")]
    public static string FahrenheitToCelsius(
        double fahrenheit)
    {
        var c = (fahrenheit - 32) * 5.0 / 9.0;
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0}F = {1:F1}C", fahrenheit, c);
    }
}
