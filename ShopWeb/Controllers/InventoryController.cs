using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    [Authorize]
    public class InventoryController : BaseController
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IInventoryService inventoryService;
		//private readonly IProductService productService;
		public InventoryController(ILogger<InventoryController> _logger, IInventoryService _inventoryService)//, IProductService _productService
		{
            logger = _logger;
            inventoryService = _inventoryService;
			//productService = _productService;
		}
        public async Task<IActionResult> Index()
        {
			var (success, inventories) = await TryExecuteAsync(() => inventoryService.AllInventories());
			if (!success)
				return View(new List<InventoryVm>());
            return View(inventories);
        }
		[HttpGet]
		public IActionResult CreateInventory()
		{
			return View(new NewInventoryVm());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateInventory(NewInventoryVm newInventoryVm)
		{
			if(!ModelState.IsValid)
			{
				return View(newInventoryVm);
			}
			var (success, inventoryId) = await TryExecuteAsync(() => inventoryService.AddInventory(newInventoryVm));
			if(!success)
			{
				return View(newInventoryVm);
			}
			return RedirectToAction("Index");
		}

	}
}
