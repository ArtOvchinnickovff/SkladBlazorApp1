using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ArticleNumber { get; set; } = string.Empty;   // Артикул

        public string Unit { get; set; } = "шт"; // шт / м / упак

        public decimal Price { get; set; }

        public int Quantity { get; set; } // количество на складе

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? CharacteristicsJson { get; set; } // специфичные характеристики
    }
}
