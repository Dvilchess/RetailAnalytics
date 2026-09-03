using Microsoft.EntityFrameworkCore;
using RetailAnalytics.API.Models;

namespace RetailAnalytics.API.Data;

public class RetailDbContext : DbContext
{
    public RetailDbContext(DbContextOptions<RetailDbContext> options) 
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones de relaciones
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Sale)
            .WithMany(s => s.SaleItems)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Product)
            .WithMany(p => p.SaleItems)
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para optimización
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        modelBuilder.Entity<Sale>()
            .HasIndex(s => s.SaleDate);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // Datos semilla
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Categorías
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Electrónicos", Description = "Productos electrónicos" },
            new Category { CategoryId = 2, Name = "Ropa", Description = "Vestimenta y accesorios" },
            new Category { CategoryId = 3, Name = "Hogar", Description = "Artículos para el hogar" }
        );

        // Productos
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Laptop HP", SKU = "ELEC-001", Price = 899990, CategoryId = 1, StockQuantity = 50, CreatedAt = DateTime.UtcNow },
            new Product { ProductId = 2, Name = "Smartphone Samsung", SKU = "ELEC-002", Price = 499990, CategoryId = 1, StockQuantity = 100, CreatedAt = DateTime.UtcNow },
            new Product { ProductId = 3, Name = "Camisa Casual", SKU = "ROPA-001", Price = 25990, CategoryId = 2, StockQuantity = 200, CreatedAt = DateTime.UtcNow },
            new Product { ProductId = 4, Name = "Lámpara LED", SKU = "HOG-001", Price = 15990, CategoryId = 3, StockQuantity = 75, CreatedAt = DateTime.UtcNow }
        );

        // Clientes
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, Name = "Juan Pérez", Email = "juan.perez@email.com", Phone = "+56912345678", RegistrationDate = DateTime.UtcNow.AddMonths(-6) },
            new Customer { CustomerId = 2, Name = "María González", Email = "maria.gonzalez@email.com", Phone = "+56987654321", RegistrationDate = DateTime.UtcNow.AddMonths(-3) }
        );
    }
}