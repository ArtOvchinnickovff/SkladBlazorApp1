using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using Microsoft.EntityFrameworkCore;




namespace SkladBlazorApp.Server.Services.Validators
{
    public interface IUserValidator
    {
        Task ValidateNewUserAsync(User user);
    }

    public class UserValidator : IUserValidator
    {
        private readonly SkladDbContext _context;

        public UserValidator(SkladDbContext context)
        {
            _context = context;
        }

        public async Task ValidateNewUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                throw new Exception("Login не может быть пустым");

            if (await _context.Users.AnyAsync(u => u.Login == user.Login))
                throw new Exception("Пользователь с таким логином уже существует");
        }
    }
}
