using ModelContextProtocol.Server;
using System.ComponentModel;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
}

public record Product(int Id, string Name, decimal Price);

[McpServerToolType]
public class ProductTools(IProductRepository repo)
{
    [McpServerTool]
    [Description("Looks up a product by ID")]
    public async Task<string> GetProduct(int productId)
    {
        try
        {
            var product = await repo
                .GetByIdAsync(productId);

            if (product is null)
                return "Product not found.";

            return string.Format(
                System.Globalization
                    .CultureInfo.InvariantCulture,
                "{0}: ${1:F2}",
                product.Name, product.Price);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
