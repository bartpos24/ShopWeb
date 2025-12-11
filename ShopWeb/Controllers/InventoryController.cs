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
    public class InventoryController : Controller
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
			//try
			//{
			//	var product = await productService.GetProductByBarcode("5449000034519");
			//	var c = product;
			//}
			//catch (Exception ex)
			//{
			//	if (ex is ApiException)
			//	{
			//		ex = ex as ApiException;
			//		return View(new ErrorViewModel { RequestId = $"Wystąpił błąd: {ex.Message}" });
			//	}
			//	logger.LogError(ex, "Error in HomeController Index");
			//}
			try
			{
				var inventories = await inventoryService.AllInventories();
				return View(inventories);
			}
			catch (Exception ex)
			{
				var x = ex;
			}
			return View(new List<InventoryVm>());
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
			try
			{
				var id = await inventoryService.AddInventory(newInventoryVm);
				return RedirectToAction("Index");
			} catch(Exception ex)
			{
				var x = ex;
				return View(newInventoryVm);
			}
			
		}

	}
}
