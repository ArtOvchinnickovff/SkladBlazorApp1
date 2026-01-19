using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SkladBlazorApp.Shared.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Manager"; // Admin / Manager
    }
}
