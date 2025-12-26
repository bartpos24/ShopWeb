using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> AllInventories();
        Task<Inventory> GetInventoryById(int inventoryId);
        Task<int> AddInventory(Inventory inventory);
        Task<List<SummaryInventoryPosition>> GetInventorySummary(int inventoryId);
        Task<CommonInventoryPosition> AddCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition);
        Task<CommonInventoryPosition> EditCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition);
        Task<int> DeleteCommonInventoryPosition(CommonInventoryPosition commonInventoryPosition);
        Task<List<CommonInventoryPosition>> GetAllCommonInventoryPositionsForUser(int inventoryId, int? userId = null);
    }
}
