using EnterpriseServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                Name = "Alice Johnson",
                Email = "alice@example.com",
                Phone = "555-0101"
            },
            new Customer
            {
                Id = 2,
                Name = "Bob Smith",
                Email = "bob@example.com",
                Phone = "555-0102"
            },
            new Customer
            {
                Id = 3,
                Name = "Carol White",
                Email = "carol@example.com"
            }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerId = 1,
                Product = "Widget Pro",
                Amount = 29.99m,
                Status = "Shipped",
                OrderDate = new DateTime(
                    2025, 1, 15,
                    0, 0, 0,
                    DateTimeKind.Utc)
            },
            new Order
            {
                Id = 2,
                CustomerId = 1,
                Product = "Gadget Plus",
                Amount = 49.99m,
                Status = "Delivered",
                OrderDate = new DateTime(
                    2025, 2, 20,
                    0, 0, 0,
                    DateTimeKind.Utc)
            },
            new Order
            {
                Id = 3,
                CustomerId = 2,
                Product = "Widget Basic",
                Amount = 14.99m,
                Status = "Pending",
                OrderDate = new DateTime(
                    2025, 3, 10,
                    0, 0, 0,
                    DateTimeKind.Utc)
            }
        );
    }
}
