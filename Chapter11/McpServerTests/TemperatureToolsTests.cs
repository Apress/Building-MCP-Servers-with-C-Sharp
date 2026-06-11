using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TemperatureToolsTests
{
    [TestMethod]
    public void CelsiusToFahrenheit_BoilingPoint_Returns212()
    {
        var result = TemperatureTools
            .CelsiusToFahrenheit(100);

        Assert.AreEqual("100C = 212.0F", result);
    }

    [TestMethod]
    public void CelsiusToFahrenheit_FreezingPoint_Returns32()
    {
        var result = TemperatureTools
            .CelsiusToFahrenheit(0);

        Assert.AreEqual("0C = 32.0F", result);
    }

    [DataTestMethod]
    [DataRow(32, "32F = 0.0C")]
    [DataRow(212, "212F = 100.0C")]
    [DataRow(-40, "-40F = -40.0C")]
    public void FahrenheitToCelsius_ReturnsExpected(
        double input, string expected)
    {
        var result = TemperatureTools
            .FahrenheitToCelsius(input);

        Assert.AreEqual(expected, result);
    }
}
