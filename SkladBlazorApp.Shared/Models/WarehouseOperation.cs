using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.Models
{
    public class WarehouseOperation
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int Quantity { get; set; }  // + приход, - расход

        public string Type { get; set; } = string.Empty; // "Приход", "Расход"

        public string? Comment { get; set; }
    }
}
