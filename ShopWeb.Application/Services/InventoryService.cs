using ShopWeb.Application.Interfaces;
using ShopWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
