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
        public async Task<int> AddInventory(Inventory inventory)
        {
            var createdInventoryId = await inventoryApi.ApiInventoryCreateInventoryPostAsync(inventory);
            return createdInventoryId;
		}
        public async Task<List<SummaryInventoryPosition>> GetInventorySummary(int inventoryId)
        {
            return await inventoryApi.ApiInventoryGetAllSummaryPositionsGetAsync(inventoryId);
        }
        public async Task<CommonInventoryPosition> AddCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition)
        {
            return await inventoryApi.ApiInventoryAddCommonInventoryPositionPostAsync(commonInventoryPosition);
        }
        public async Task<List<CommonInventoryPosition>> GetAllCommonInventoryPositionsForUser(int inventoryId, int? userId = null)
        {
            return await inventoryApi.ApiInventoryGetAllCommonInventoryPositionsForUserGetAsync(inventoryId, userId);
        }
        public async Task<CommonInventoryPosition> EditCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition)
        {
            return await inventoryApi.ApiInventoryEditCommonInventoryPositionPostAsync(commonInventoryPosition);
        }
        public async Task<int> DeleteCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition)
        {
            return await inventoryApi.ApiInventoryDeleteCommonInventoryPositionPostAsync(commonInventoryPosition);
        }
    }
}
