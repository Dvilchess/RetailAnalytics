using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailAnalytics.API.Data;
using RetailAnalytics.API.DTOs;
using RetailAnalytics.API.Models;

namespace RetailAnalytics.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly RetailDbContext _context;
    private readonly ILogger<SalesController> _logger;

    public SalesController(RetailDbContext context, ILogger<SalesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales(
        [FromQuery] DateTime? fromDate, 
        [FromQuery] DateTime? toDate)
    {
        try
        {
            var query = _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p!.Category)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(s => s.SaleDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(s => s.SaleDate <= toDate.Value);

            var sales = await query.OrderByDescending(s => s.SaleDate).ToListAsync();
            return Ok(sales);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ventas");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sale>> GetSale(int id)
    {
        try
        {
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(s => s.SaleId == id);
            
            if (sale == null)
                return NotFound($"Venta con ID {id} no encontrada");
            
            return Ok(sale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener venta {id}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Sale>> CreateSale([FromBody] SaleCreateDTO saleDTO)
    {
        try
        {
            if (saleDTO == null || saleDTO.SaleItems == null || !saleDTO.SaleItems.Any())
                return BadRequest("La venta debe incluir al menos un producto");

            var sale = new Sale
            {
                CustomerId = saleDTO.CustomerId,
                PaymentMethod = saleDTO.PaymentMethod,
                Discount = saleDTO.Discount,
                SaleDate = DateTime.UtcNow,
                Status = "Completed",
                SaleItems = new List<SaleItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in saleDTO.SaleItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return BadRequest($"Producto {item.ProductId} no encontrado");

                if (product.StockQuantity < item.Quantity)
                    return BadRequest($"Stock insuficiente para {product.Name}");

                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = item.Quantity * product.Price
                };

                sale.SaleItems.Add(saleItem);
                totalAmount += saleItem.Subtotal;

                // Actualizar stock
                product.StockQuantity -= item.Quantity;
            }

            sale.TotalAmount = totalAmount - sale.Discount;
            
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSale), new { id = sale.SaleId }, sale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear venta");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("analytics")]
    public async Task<ActionResult<IEnumerable<SalesAnalytics>>> GetAnalytics(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        try
        {
            var query = _context.SaleItems
                .Include(si => si.Product)
                    .ThenInclude(p => p!.Category)
                .Include(si => si.Sale)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(si => si.Sale.SaleDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(si => si.Sale.SaleDate <= toDate.Value);

            var analytics = await query
                .GroupBy(si => new { si.Product!.Category!.Name, si.Sale.SaleDate.Date })
                .Select(g => new SalesAnalytics
                {
                    CategoryName = g.Key.Name,
                    TotalSales = g.Sum(si => si.Subtotal),
                    TotalProductsSold = g.Sum(si => si.Quantity),
                    AverageSaleValue = g.Average(si => si.Subtotal),
                    Date = g.Key.Date
                })
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener analíticas");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}