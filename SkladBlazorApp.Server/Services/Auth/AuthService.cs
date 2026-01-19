using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkladBlazorApp.Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SkladDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(SkladDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user == null)
                throw new Exception("Неверный логин или пароль");

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Неверный логин или пароль");

            return GenerateJwt(user);
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
