using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopWeb.Application.Extensions;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Domain.Interfaces;

namespace ShopWeb.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IMapper mapper;
        private readonly SheetGenerator sheetGenerator;
        public InventoryService(IInventoryRepository _inventoryRepository, IMapper _mapper)
        {
            inventoryRepository = _inventoryRepository;
            mapper = _mapper;
            sheetGenerator = new SheetGenerator();
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
        public async Task<InventorySummaryVm> GetInventorySummary(int inventoryId)
        {
            var summaryPositions = await inventoryRepository.GetInventorySummary(inventoryId);
            return new InventorySummaryVm()
            {
                Positions = mapper.Map<List<SummaryInventoryPositionVm>>(summaryPositions),
                Count = summaryPositions.Count
            };
        }
        public async Task<byte[]> GenerateSheet(InventorySummaryVm inventorySummaryVm)
        {
            return await sheetGenerator.GenerateSheet(inventorySummaryVm);
        }
        public async Task<CommonInventoryPositionVm> AddCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm)
        {
            var commonInventoryPosition = mapper.Map<Domain.Models.CommonInventoryPosition>(commonInventoryPositionVm);
            var createdPosition = await inventoryRepository.AddCommonInventoryPosition(commonInventoryPosition);
            return mapper.Map<CommonInventoryPositionVm>(createdPosition);
        }
        public async Task<List<CommonInventoryPositionVm>> GetAllCommonInventoryPositionsForUser(int inventoryId, int? userId = null)
        {
            var positionsForUser = await inventoryRepository.GetAllCommonInventoryPositionsForUser(inventoryId, userId);
            return mapper.Map<List<CommonInventoryPositionVm>>(positionsForUser);
        }
    }
}
