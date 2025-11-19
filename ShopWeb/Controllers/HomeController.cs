using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Interfaces;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IInventoryService inventoryService;

        public HomeController(ILogger<HomeController> _logger, IInventoryService _inventoryService)
        {
            logger = _logger; 
            inventoryService = _inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var x = await inventoryService.Login("barpos", "Dobrakow56!","");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
