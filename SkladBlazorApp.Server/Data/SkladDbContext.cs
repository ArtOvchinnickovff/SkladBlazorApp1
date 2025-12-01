using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Shared;
using SkladBlazorApp.Shared.Models;


namespace SkladBlazorApp.Server.Data
{
        public class SkladDbContext : DbContext
        {
            public SkladDbContext(DbContextOptions<SkladDbContext> options)
                : base(options)
            {
            }

            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
             public DbSet<WarehouseOperation> WarehouseOperations { get; set; }
               public DbSet<User> Users { get; set; }

        } 
}

