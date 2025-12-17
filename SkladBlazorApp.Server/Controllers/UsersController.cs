using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Shared.ModelsDTO;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly SkladDbContext _context;

        public UsersController(SkladDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll() =>
            await _context.Users.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(CreateUsersDto userDto)
        {
            var user = new User
            {
                Login = userDto.Login,
                PasswordHash = userDto.Password,
                Role = userDto.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, CreateUsersDto userDto)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Login = userDto.Login;
            existing.PasswordHash = userDto.Password;
            existing.Role = userDto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}