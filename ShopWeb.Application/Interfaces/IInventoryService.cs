using ShopWeb.Application.TransferObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<List<InventoryVm>> AllInventories();
        Task<int> AddInventory(NewInventoryVm newInventoryVm);
        Task<InventorySummaryVm> GetInventorySummary(int inventoryId);
        Task<byte[]> GenerateSheet(InventorySummaryVm inventorySummaryVm);
        Task<CommonInventoryPositionVm> AddCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm);
        Task<CommonInventoryPositionVm> EditCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm);
        Task<int> DeleteCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm);
        Task<List<CommonInventoryPositionVm>> GetAllCommonInventoryPositionsForUser(int inventoryId, int? userId = null);
    }
}
