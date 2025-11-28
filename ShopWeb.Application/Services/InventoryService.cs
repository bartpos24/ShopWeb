using ShopWeb.Application.Interfaces;
using ShopWeb.Domain.Interfaces;

namespace ShopWeb.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository inventoryRepository;
        public InventoryService(IInventoryRepository _inventoryRepository)
        {
            inventoryRepository = _inventoryRepository;
        }

        public async Task<string> Login(string username, string password, string ssaid = null)
        {
            return await inventoryRepository.Login(username, password, ssaid);
        }
    }
}
