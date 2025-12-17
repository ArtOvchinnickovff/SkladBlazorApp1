using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO;
using Microsoft.EntityFrameworkCore;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SkladDbContext _context;

        public CategoriesController(SkladDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAll() =>
            await _context.Categories.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDto dto)
        {
            var existing = await _context.Categories.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
