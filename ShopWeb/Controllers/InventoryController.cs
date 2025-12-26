using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Application.TransferObjects.Products;
using ShopWeb.Extensions;
using ShopWeb.Models;
using System.Threading.Tasks;

namespace ShopWeb.Controllers
{
    [Authorize]
    public class InventoryController : BaseController
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IInventoryService inventoryService;
        private readonly IProductService productService;
        private const string SessionKeyInventoryPositions = "_InventoryPositions";
        
        public InventoryController(ILogger<InventoryController> _logger, IInventoryService _inventoryService, IProductService _productService)
		{
            logger = _logger;
            inventoryService = _inventoryService;
			productService = _productService;
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
        public async Task<IActionResult> InventorySummary(int inventoryId)
		{
            var (successInventory, inventory) = await TryExecuteAsync(() => inventoryService.GetInventoryById(inventoryId));
            if (!successInventory || inventory == null)
                return RedirectToAction("Index");
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

        [HttpGet]
		public async Task<IActionResult> Inventory(int inventoryId)
		{
            var (successInventory, inventory) = await TryExecuteAsync(() => inventoryService.GetInventoryById(inventoryId));
            if (!successInventory || inventory == null)
                return RedirectToAction("Index");

            var (successUnits, units) = await TryExecuteAsync(() => productService.GetAllUnits());
            if (!successUnits || units == null)
            {
                // Fallback to empty list if units can't be loaded
                units = new List<ProductUnitVm>();
            }

            List<CommonInventoryPositionVm> positions = new List<CommonInventoryPositionVm>();
            var (success, positionsForUser) = await TryExecuteAsync(() => inventoryService.GetAllCommonInventoryPositionsForUser(inventoryId));
            if (positionsForUser != null)
                positions = positionsForUser.OrderByDescending(w => w.ScanDate).ToList();
            var sessionKey = $"{SessionKeyInventoryPositions}_{inventoryId}";
            HttpContext.Session.SetObject(sessionKey, positions);
            ViewBag.Inventory = inventory;
            ViewBag.Positions = positions;
            ViewBag.Units = units;

            return View(new CommonInventoryPositionVm { InventoryId = inventoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInventoryPosition([FromBody] CommonInventoryPositionVm positionVm)
        {
            positionVm.ScanDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { success = false, message = $"Nieprawidłowe dane: {errors}" });
            }

            var (success, addedPosition) = await TryExecuteAsync(() => inventoryService.AddCommonInventoryPosition(positionVm));

            if (!success)
            {
                return Json(new { success = false, message = "Nie udało się dodać pozycji" });
            }

            // Add to session list
            var sessionKey = $"{SessionKeyInventoryPositions}_{positionVm.InventoryId}";
            var positions = HttpContext.Session.GetObject<List<CommonInventoryPositionVm>>(sessionKey)
                ?? new List<CommonInventoryPositionVm>();

            positions.Add(addedPosition);
            HttpContext.Session.SetObject(sessionKey, positions);

            return Json(new
            {
                success = true,
                position = addedPosition,
                message = "Pozycja została dodana pomyślnie"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInventoryPosition([FromBody] CommonInventoryPositionVm positionVm)
        {
            positionVm.ScanDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { success = false, message = $"Nieprawidłowe dane: {errors}" });
            }

            var (success, editedPosition) = await TryExecuteAsync(() => inventoryService.EditCommonInventoryPosition(positionVm));

            if (!success || editedPosition == null)
            {
                return Json(new { success = false, message = "Nie udało się edytować pozycji" });
            }

            // Add to session list
            var sessionKey = $"{SessionKeyInventoryPositions}_{positionVm.InventoryId}";
            var positions = HttpContext.Session.GetObject<List<CommonInventoryPositionVm>>(sessionKey)
                ?? new List<CommonInventoryPositionVm>();

            var existingPosition = positions.FirstOrDefault(w => w.Id == editedPosition.Id);
            if (existingPosition != null)
            {
                positions.Remove(existingPosition);
            }
            positions.Add(editedPosition);
            HttpContext.Session.SetObject(sessionKey, positions);

            return Json(new
            {
                success = true,
                position = editedPosition,
                message = "Pozycja została zaktualizowana pomyślnie"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInventoryPosition([FromBody] CommonInventoryPositionVm positionVm)
        {
            var sessionKey = $"{SessionKeyInventoryPositions}_{positionVm.InventoryId}";
            var positions = HttpContext.Session.GetObject<List<CommonInventoryPositionVm>>(sessionKey)
                ?? new List<CommonInventoryPositionVm>();
            var positionToRemoveFromList = positions.FirstOrDefault(w => w.Id == positionVm.Id && w.InventoryId == positionVm.InventoryId);
            if (positionToRemoveFromList == null)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { success = false, message = $"Nieprawidłowe dane: {errors}" });
            }

            var (success, deletedPositionId) = await TryExecuteAsync(() => inventoryService.DeleteCommonInventoryPosition(positionToRemoveFromList));

            if (!success)
            {
                return Json(new { success = false, message = "Nie udało się usunąć pozycji" });
            }

            // Add to session list
            
            if (positionToRemoveFromList != null)
            {
                positions.Remove(positionToRemoveFromList);
                HttpContext.Session.SetObject(sessionKey, positions);
            }

            return Json(new
            {
                success = true,
                position = positionToRemoveFromList,
                message = "Pozycja została usunięta pomyślnie"
            });
        }

        [HttpPost]
        public IActionResult ClearInventoryPositions(int inventoryId)
        {
            var sessionKey = $"{SessionKeyInventoryPositions}_{inventoryId}";
            HttpContext.Session.Remove(sessionKey);
            return Json(new { success = true });
        }
    }
}
