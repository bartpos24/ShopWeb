using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopWeb.Application.Extensions;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;

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
            var inventories = await inventoryRepository.AllInventories();
            return mapper.Map<List<InventoryVm>>(inventories);
        }
        public async Task<InventoryVm> GetInventoryById(int inventoryId)
        {
            var inventory = await inventoryRepository.GetInventoryById(inventoryId);
            return mapper.Map<InventoryVm>(inventory);
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
        public async Task<InventorySummaryVm> GetInventorySummary(int inventoryId, int pageSize, int pageNo, string searchString)
        {
            var result = await inventoryRepository.GetInventorySummary(inventoryId);
            var summaryPositions = result.Where(w => w.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            var summaryToShow = summaryPositions.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
            return new InventorySummaryVm()
            {
                Positions = mapper.Map<List<SummaryInventoryPositionVm>>(summaryToShow),
                Count = summaryPositions.Count,
                PageSize = pageSize,
                CurrentPage = pageNo,
                SearchString = searchString
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
        public async Task<CommonInventoryPositionVm> EditCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm)
        {
            var commonInventoryPosition = mapper.Map<Domain.Models.CommonInventoryPosition>(commonInventoryPositionVm);
            var editedPosition = await inventoryRepository.EditCommonInventoryPosition(commonInventoryPosition);
            return mapper.Map<CommonInventoryPositionVm>(editedPosition);
        }
        public async Task<int> DeleteCommonInventoryPosition(CommonInventoryPositionVm commonInventoryPositionVm)
        {
            var commonInventoryPosition = mapper.Map<Domain.Models.CommonInventoryPosition>(commonInventoryPositionVm);
            return await inventoryRepository.DeleteCommonInventoryPosition(commonInventoryPosition);
        }
    }
}
