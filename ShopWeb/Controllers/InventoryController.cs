using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Interfaces;

namespace ShopWeb.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IInventoryService inventoryService;
        public InventoryController(ILogger<InventoryController> _logger, IInventoryService _inventoryService)
        {
            logger = _logger;
            inventoryService = _inventoryService;
        }
        public async Task<IActionResult> Index()
        {
            var inventories = await inventoryService.AllInventories();
            return View(inventories);
        }
    }
}
