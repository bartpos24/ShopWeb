using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api;

namespace ShopWeb.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IInventoryApi inventoryApi;
        public InventoryRepository(IInventoryApi _inventoryApi)
        {
            inventoryApi = _inventoryApi;
        }

        public async Task<List<Inventory>> AllInventories()
        {
            return await inventoryApi.ApiInventoryGetAllInventoriesGetAsync();
        }
    }
}
