using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailAnalytics.API.Data;
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
                        .ThenInclude(p => p.Category)
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
            return StatusCode(500, "Error interno del servidor");
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
                    .ThenInclude(p => p.Category)
                .Include(si => si.Sale)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(si => si.Sale.SaleDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(si => si.Sale.SaleDate <= toDate.Value);

            var analytics = await query
                .GroupBy(si => new { si.Product.Category.Name, si.Sale.SaleDate.Date })
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
            return StatusCode(500, "Error interno del servidor");
        }
    }
}