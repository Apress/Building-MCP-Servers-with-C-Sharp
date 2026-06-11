using System.ComponentModel;
using EnterpriseServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace EnterpriseServer.Tools;

[McpServerToolType]
public static class CustomerTools
{
    [McpServerTool]
    [Description("Look up a customer by name or email. "
        + "Returns customer details including contact information.")]
    public static async Task<string> CustomerLookup(
        AppDbContext db,
        ILogger<AppDbContext> logger,
        [Description("Search term: partial name or email address")] string searchTerm)
    {
        logger.LogInformation("Looking up customer: {Search}", searchTerm);

        var customers = await db.Customers
            .Where(c => c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm))
            .ToListAsync();

        if (customers.Count == 0)
            return $"No customers found matching '{searchTerm}'.";

        var results = customers.Select(c =>
            $"ID: {c.Id}, Name: {c.Name}, " +
            $"Email: {c.Email}, " +
            $"Phone: {c.Phone ?? "N/A"}");

        return string.Join("\n", results);
    }

    [McpServerTool]
    [Description("Get the order history for a customer. "
        + "Returns all orders with status and amounts.")]
    public static async Task<string> OrderHistory(
        AppDbContext db,
        ILogger<AppDbContext> logger,
        [Description("The customer ID to look up")] int customerId)
    {
        logger.LogInformation("Fetching orders for customer {Id}", customerId);

        var customer = await db.Customers.FindAsync(customerId);

        if (customer is null)
            return $"Customer {customerId} not found.";

        var orders = await db.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        if (orders.Count == 0)
            return $"No orders found for {customer.Name}.";

        var header = $"Orders for {customer.Name}:\n";
        var lines = orders.Select(o =>
            $"  Order #{o.Id}: {o.Product} " +
            $"- ${o.Amount} ({o.Status}) " +
            $"on {o.OrderDate:yyyy-MM-dd}");

        return header + string.Join("\n", lines);
    }
}
