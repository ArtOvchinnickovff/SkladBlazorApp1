using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseOperationsController : ControllerBase
    {
        private readonly SkladDbContext _context;

        public WarehouseOperationsController(SkladDbContext context)
        {
            _context = context;
        }

        // ======== ПРОДУКТЫ ========
        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetProducts(string? search = null, int? categoryId = null)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            return await query.ToListAsync();
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return product;
        }

        [HttpPost("products")]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Quantity = product.Quantity;
            existing.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======== КАТЕГОРИИ ========
        [HttpGet("categories")]
        public async Task<IEnumerable<Category>> GetCategories() =>
            await _context.Categories.ToListAsync();

        [HttpPost("categories")]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }

        // ======== ОПЕРАЦИИ СКЛАДА ========
        [HttpPost("products/{id}/adjust")]
        public async Task<IActionResult> AdjustQuantity(int id, int delta, string type = "manual", string? comment = null)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Quantity += delta;

            // Логируем операцию
            var operation = new WarehouseOperation
            {
                ProductId = id,
                Quantity = delta,
                Type = type,
                Comment = comment,
                Date = DateTime.Now
            };
            _context.WarehouseOperations.Add(operation);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Получение операций
        [HttpGet("operations")]
        public async Task<IEnumerable<WarehouseOperation>> GetOperations(int? productId = null)
        {
            var query = _context.WarehouseOperations.Include(o => o.Product).AsQueryable();
            if (productId.HasValue)
                query = query.Where(o => o.ProductId == productId.Value);

            return await query.OrderByDescending(o => o.Date).ToListAsync();
        }
    }
}
