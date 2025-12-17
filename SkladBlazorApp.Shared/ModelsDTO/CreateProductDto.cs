using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.ModelsDTO
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string ArticleNumber { get; set; } = string.Empty;
        public string Unit { get; set; } = "м";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string? CharacteristicsJson { get; set; }
    }
}
