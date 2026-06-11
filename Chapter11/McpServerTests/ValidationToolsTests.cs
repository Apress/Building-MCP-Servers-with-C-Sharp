using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ValidationToolsTests
{
    [TestMethod]
    public void GetPageInfo_NegativeInput_ReturnsError()
    {
        var result = ValidationTools.GetPageInfo(-1);

        StringAssert.Contains(
            result, "must be positive");
    }

    [TestMethod]
    public void GetPageInfo_Zero_ReturnsError()
    {
        var result = ValidationTools.GetPageInfo(0);

        StringAssert.Contains(
            result, "must be positive");
    }

    [TestMethod]
    public void GetPageInfo_ValidInput_ReturnsInfo()
    {
        var result = ValidationTools.GetPageInfo(5);

        StringAssert.Contains(result, "Page 5");
    }
}
