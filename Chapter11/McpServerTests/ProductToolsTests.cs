using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class ProductToolsTests
{
    private readonly Mock<IProductRepository>
        _mockRepo = new();
    private readonly ProductTools _tools;

    public ProductToolsTests()
    {
        _tools = new ProductTools(_mockRepo.Object);
    }

    [TestMethod]
    public async Task GetProduct_Exists_ReturnsFormatted()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync(
                new Product(42, "Widget", 9.99m));

        var result = await _tools.GetProduct(42);

        Assert.AreEqual("Widget: $9.99", result);
    }

    [TestMethod]
    public async Task GetProduct_NotFound_ReturnsMessage()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        var result = await _tools.GetProduct(99);

        Assert.AreEqual("Product not found.", result);
    }

    [TestMethod]
    public async Task GetProduct_RepositoryThrows_ReturnsError()
    {
        _mockRepo.Setup(r =>
            r.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "DB connection lost"));

        var result = await _tools.GetProduct(1);

        StringAssert.Contains(
            result.ToLower(), "error");
    }
}
