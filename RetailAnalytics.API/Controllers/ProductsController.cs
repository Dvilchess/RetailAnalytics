using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailAnalytics.API.Data;
using RetailAnalytics.API.Models;

namespace RetailAnalytics.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly RetailDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(RetailDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        try
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
            
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            
            if (product == null)
                return NotFound($"Producto con ID {id} no encontrado");
            
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener producto {id}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}