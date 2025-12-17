using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseOperationsController : ControllerBase
    {
        private readonly SkladDbContext _context;

        public WarehouseOperationsController(SkladDbContext context)
        {
            _context = context;
        }

        // ======== Приёмка товара ========
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveProduct(WarehouseActionDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return NotFound("Продукт не найден");

            product.Quantity += dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Type = "Приход",
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======== Списание товара ========
        [HttpPost("consume")]
        public async Task<IActionResult> ConsumeProduct(WarehouseActionDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return NotFound("Продукт не найден");

            if (dto.Quantity > product.Quantity)
                return BadRequest("Недостаточно товара на складе");

            product.Quantity -= dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = -dto.Quantity,
                Type = "Расход",
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======== Инвентаризация ========
        [HttpPost("inventory")]
        public async Task<IActionResult> InventoryProduct(WarehouseActionDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return NotFound("Продукт не найден");

            var delta = dto.Quantity - product.Quantity; // delta между фактом и текущим остатком
            product.Quantity = dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = delta,
                Type = "Инвентаризация",
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ======== Остатки товаров ========
        [HttpGet("balance")]
        public async Task<IEnumerable<ProductBalanceDto>> GetBalance()
        {
            return await _context.Products
                .Select(p => new ProductBalanceDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Quantity = p.Quantity,
                    Unit = p.Unit
                })
                .ToListAsync();
        }

        // ======== История операций по продукту ========
        [HttpGet("history/{productId}")]
        public async Task<IEnumerable<WarehouseOperationDto>> GetHistory(int productId)
        {
            return await _context.WarehouseOperations
                .Where(o => o.ProductId == productId)
                .OrderByDescending(o => o.Date)
                .Select(o => new WarehouseOperationDto
                {
                    Date = o.Date,
                    Quantity = o.Quantity,
                    Type = o.Type,
                    Comment = o.Comment,
                    ProductName = o.Product!.Name
                })
                .ToListAsync();
        }
    }
}
