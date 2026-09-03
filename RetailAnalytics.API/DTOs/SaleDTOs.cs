namespace RetailAnalytics.API.DTOs;

public class SaleCreateDTO
{
    public int CustomerId { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public decimal Discount { get; set; } = 0;
    public List<SaleItemDTO> SaleItems { get; set; } = new();
}

public class SaleItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}