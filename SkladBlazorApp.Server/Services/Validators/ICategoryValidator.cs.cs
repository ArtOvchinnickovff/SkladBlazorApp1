using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace SkladBlazorApp.Server.Services.Validators
{
    public interface ICategoryValidator
    {
        Task ValidateNewCategoryAsync(Category category);
    }

    public class CategoryValidator : ICategoryValidator
    {
        private readonly SkladDbContext _context;

        public CategoryValidator(SkladDbContext context)
        {
            _context = context;
        }

        public async Task ValidateNewCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Название категории не может быть пустым");

            if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
                throw new Exception("Категория с таким названием уже существует");
        }
    }
}
