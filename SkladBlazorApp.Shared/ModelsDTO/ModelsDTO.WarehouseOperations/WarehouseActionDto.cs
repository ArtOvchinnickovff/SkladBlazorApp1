using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations
{
    public class WarehouseActionDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } // Для инвентаризации - это новое количество
        public string? Comment { get; set; }
    }
}
