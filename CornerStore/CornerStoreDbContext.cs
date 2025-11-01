using Microsoft.EntityFrameworkCore;
using CornerStore.Models;

namespace CornerStore;

public class CornerStoreDbContext(DbContextOptions<CornerStoreDbContext> context) : DbContext(context)
{
    public DbSet<Cashier> Cashiers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    //allows us to configure the schema when migrating as well as seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure OrderProduct composite key
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        // Configure relationships
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId);

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, CategoryName = "Beverages" },
            new Category { Id = 2, CategoryName = "Snacks" },
            new Category { Id = 3, CategoryName = "Dairy" },
            new Category { Id = 4, CategoryName = "Household" }
        );

        // Seed Cashiers
        modelBuilder.Entity<Cashier>().HasData(
            new Cashier { Id = 1, FirstName = "John", LastName = "Doe" },
            new Cashier { Id = 2, FirstName = "Jane", LastName = "Smith" },
            new Cashier { Id = 3, FirstName = "Bob", LastName = "Johnson" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Coca Cola", Brand = "Coca-Cola", Price = 1.99m, CategoryId = 1 },
            new Product { Id = 2, ProductName = "Pepsi", Brand = "PepsiCo", Price = 1.89m, CategoryId = 1 },
            new Product { Id = 3, ProductName = "Sprite", Brand = "Coca-Cola", Price = 1.79m, CategoryId = 1 },
            new Product { Id = 4, ProductName = "Doritos", Brand = "Frito-Lay", Price = 3.49m, CategoryId = 2 },
            new Product { Id = 5, ProductName = "Lays Chips", Brand = "Frito-Lay", Price = 2.99m, CategoryId = 2 },
            new Product { Id = 6, ProductName = "Milk", Brand = "Horizon", Price = 4.99m, CategoryId = 3 },
            new Product { Id = 7, ProductName = "Cheese", Brand = "Kraft", Price = 5.49m, CategoryId = 3 },
            new Product { Id = 8, ProductName = "Paper Towels", Brand = "Bounty", Price = 6.99m, CategoryId = 4 },
            new Product { Id = 9, ProductName = "Dish Soap", Brand = "Dawn", Price = 3.99m, CategoryId = 4 }
        );

        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, CashierId = 1, PaidOnDate = new DateTime(2024, 1, 15, 10, 30, 0) },
            new Order { Id = 2, CashierId = 1, PaidOnDate = new DateTime(2024, 1, 16, 14, 20, 0) },
            new Order { Id = 3, CashierId = 2, PaidOnDate = new DateTime(2024, 1, 16, 9, 15, 0) },
            new Order { Id = 4, CashierId = 2, PaidOnDate = null }, // Unpaid order
            new Order { Id = 5, CashierId = 3, PaidOnDate = new DateTime(2024, 1, 17, 11, 45, 0) }
        );

        // Seed OrderProducts
        modelBuilder.Entity<OrderProduct>().HasData(
            // Order 1 - Multiple items
            new OrderProduct { OrderId = 1, ProductId = 1, Quantity = 2 },
            new OrderProduct { OrderId = 1, ProductId = 4, Quantity = 1 },
            new OrderProduct { OrderId = 1, ProductId = 6, Quantity = 1 },

            // Order 2 - Single item
            new OrderProduct { OrderId = 2, ProductId = 2, Quantity = 3 },

            // Order 3 - Multiple items
            new OrderProduct { OrderId = 3, ProductId = 4, Quantity = 2 },
            new OrderProduct { OrderId = 3, ProductId = 5, Quantity = 2 },
            new OrderProduct { OrderId = 3, ProductId = 1, Quantity = 1 },

            // Order 4 - Unpaid order
            new OrderProduct { OrderId = 4, ProductId = 7, Quantity = 1 },
            new OrderProduct { OrderId = 4, ProductId = 8, Quantity = 1 },

            // Order 5 - Popular items
            new OrderProduct { OrderId = 5, ProductId = 1, Quantity = 5 },
            new OrderProduct { OrderId = 5, ProductId = 4, Quantity = 3 }
        );
    }
}
