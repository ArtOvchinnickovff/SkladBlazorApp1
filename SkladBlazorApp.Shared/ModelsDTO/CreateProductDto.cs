using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.ModelsDTO
{
    public class CreateProductDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ArticleNumber { get; set; } = string.Empty;
        [Required]
        public string Unit { get; set; } = "м";

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public string? CharacteristicsJson { get; set; }

     
    }
}
