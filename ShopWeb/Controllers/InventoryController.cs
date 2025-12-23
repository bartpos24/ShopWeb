using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Domain.Models;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;
using ShopWeb.Models;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> InventorySummary(InventoryVm inventory)
		{
			var (success, inventorySummary) = await TryExecuteAsync(() => inventoryService.GetInventorySummary(inventory.Id));
			if (!success)
				return RedirectToAction("Index");

			inventorySummary.Inventory = inventory;
            TempData["InventorySummary"] = System.Text.Json.JsonSerializer.Serialize(inventorySummary);
            return View(inventorySummary);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateSheet()
        {
            if (TempData["InventorySummary"] is string json)
            {
                var inventorySummary = System.Text.Json.JsonSerializer.Deserialize<InventorySummaryVm>(json);

				// TODO: Generate PDF using inventorySummary
				var (success, file) = await TryExecuteAsync(() => inventoryService.GenerateSheet(inventorySummary));
				if(!success)
					return RedirectToAction("InventorySummary", new { inventory = inventorySummary.Inventory });
				return File(file, "application/pdf", $"Inventory_{inventorySummary.Inventory.Name}.pdf");
            }

            return RedirectToAction("Index");
        }
    }
}
