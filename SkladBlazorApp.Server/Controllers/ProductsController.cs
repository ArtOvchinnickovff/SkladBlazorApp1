using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SkladDbContext _context;

        public ProductsController(SkladDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll()
        => await _context.Products.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(CreateProductDto dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                return BadRequest("Категория не существует");

            var product = new Product
            {
                Name = dto.Name,
                ArticleNumber = dto.ArticleNumber,
                Unit = dto.Unit,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                CharacteristicsJson = dto.CharacteristicsJson
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.ArticleNumber = dto.ArticleNumber;
            existing.Unit = dto.Unit;
            existing.Price = dto.Price;
            existing.Quantity = dto.Quantity;
            existing.CategoryId = dto.CategoryId;
            existing.CharacteristicsJson = dto.CharacteristicsJson;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

