using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladBlazorApp.Shared.Enums
{
    public enum WarehouseOperationType
    {
        Incoming,        // Приход
        Outgoing,        // Расход
        InventoryAdjust  // Инвентаризация
    }
}
