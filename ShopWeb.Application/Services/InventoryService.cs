using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Domain.Interfaces;

namespace ShopWeb.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IMapper mapper;
        public InventoryService(IInventoryRepository _inventoryRepository, IMapper _mapper)
        {
            inventoryRepository = _inventoryRepository;
            mapper = _mapper;
        }

        public async Task<List<InventoryVm>> AllInventories()
        {
            //return await inventoryRepository.AllInventories().ProjectTo<InventoryVm>(mapper.ConfigurationProvider);

            var inventories = await inventoryRepository.AllInventories();
            return mapper.Map<List<InventoryVm>>(inventories);
        }
        public async Task<int> AddInventory(NewInventoryVm newInventoryVm)
        {
            var inventory = mapper.Map<Domain.Models.Inventory>(newInventoryVm);
            var createdInventoryId = await inventoryRepository.AddInventory(inventory);
            return createdInventoryId;
		}
	}
}
