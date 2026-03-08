using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Shared.Exceptions;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations;
using SkladBlazorApp.Shared.Enums;

namespace SkladBlazorApp.Server.Services.Warehouse
{
    public class WarehouseService : IWarehouseService
    {
        private readonly SkladDbContext _context;

        public WarehouseService(SkladDbContext context)
        {
            _context = context;
        }

        public async Task ReceiveAsync(WarehouseActionDto dto)
        {
            if (dto.Quantity <= 0)
                throw new BusinessException("Количество должно быть больше нуля");

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new NotFoundException("Продукт не найден");

            product.Quantity += dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                OperationType = WarehouseOperationType.Incoming,
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task ConsumeAsync(WarehouseActionDto dto)
        {
            if (dto.Quantity <= 0)
                throw new BusinessException("Количество должно быть больше нуля");

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new NotFoundException("Продукт не найден");

            if (dto.Quantity > product.Quantity)
                throw new BusinessException("Недостаточно товара на складе");

            product.Quantity -= dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = -dto.Quantity,
                OperationType = WarehouseOperationType.Outgoing,
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task InventoryAsync(WarehouseActionDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new NotFoundException("Продукт не найден");

            var delta = dto.Quantity - product.Quantity;
            product.Quantity = dto.Quantity;

            _context.WarehouseOperations.Add(new WarehouseOperation
            {
                ProductId = dto.ProductId,
                Quantity = delta,
                OperationType = WarehouseOperationType.InventoryAdjust,
                Comment = dto.Comment,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductBalanceDto>> GetBalanceAsync()
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

        public async Task<List<WarehouseOperationDto>> GetHistoryAsync(int productId)
        {
            var query = _context.WarehouseOperations
                .Include(o => o.Product)
                .AsQueryable();

            if (productId != 0)
                query = query.Where(o => o.ProductId == productId);

            return await query
                .OrderByDescending(o => o.Date)
                .Select(o => new WarehouseOperationDto
                {
                    Date = o.Date,
                    Quantity = o.Quantity,
                    OperationType = o.OperationType,
                    Comment = o.Comment,
                    ProductName = o.Product!.Name
                })
                .ToListAsync();
        }
    }
}
