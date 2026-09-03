using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailAnalytics.API.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

public class Sale
{
    [Key]
    public int SaleId { get; set; }
    
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }
    
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "Cash";
    
    [MaxLength(20)]
    public string Status { get; set; } = "Completed";
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}

public class SaleItem
{
    [Key]
    public int SaleItemId { get; set; }
    
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
}

// Modelo para Business Intelligence
public class SalesAnalytics
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public int TotalProductsSold { get; set; }
    public decimal AverageSaleValue { get; set; }
    public DateTime Date { get; set; }
}