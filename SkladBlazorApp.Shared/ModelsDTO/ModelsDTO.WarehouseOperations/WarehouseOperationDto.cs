using SkladBlazorApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations
{
    public class WarehouseOperationDto
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public WarehouseOperationType OperationType { get; set; }
        public string? Comment { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
