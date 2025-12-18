using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkladBlazorApp.Server.Data;
using SkladBlazorApp.Server.Services.Warehouse;
using SkladBlazorApp.Shared.Exceptions;
using SkladBlazorApp.Shared.Models;
using SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations;

namespace SkladBlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/warehouse")]
    public class WarehouseOperationsController : ControllerBase
    {
        private readonly IWarehouseService _service;

        public WarehouseOperationsController(IWarehouseService service)
        {
            _service = service;
        }

        [HttpPost("receive")]
        public async Task<IActionResult> Receive(WarehouseActionDto dto)
        {
            await _service.ReceiveAsync(dto);
            return NoContent();
        }

        [HttpPost("consume")]
        public async Task<IActionResult> Consume(WarehouseActionDto dto)
        {
            await _service.ConsumeAsync(dto);
            return NoContent();
        }

        [HttpPost("inventory")]
        public async Task<IActionResult> Inventory(WarehouseActionDto dto)
        {
            await _service.InventoryAsync(dto);
            return NoContent();
        }

        [HttpGet("balance")]
        public async Task<IEnumerable<ProductBalanceDto>> Balance()
            => await _service.GetBalanceAsync();

        [HttpGet("history/{productId}")]
        public async Task<IEnumerable<WarehouseOperationDto>> History(int productId)
            => await _service.GetHistoryAsync(productId);
    }
}