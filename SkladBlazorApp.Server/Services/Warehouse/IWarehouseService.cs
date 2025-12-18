using SkladBlazorApp.Shared.ModelsDTO.ModelsDTO.WarehouseOperations;

namespace SkladBlazorApp.Server.Services.Warehouse
{
    public interface IWarehouseService
    {
        Task ReceiveAsync(WarehouseActionDto dto);
        Task ConsumeAsync(WarehouseActionDto dto);
        Task InventoryAsync(WarehouseActionDto dto);

        Task<List<ProductBalanceDto>> GetBalanceAsync();
        Task<List<WarehouseOperationDto>> GetHistoryAsync(int productId);
    }
}
