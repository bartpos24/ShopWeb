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
        private readonly ILoginService loginService;

        public HomeController(ILogger<HomeController> _logger, ILoginService _loginService)
        {
            logger = _logger;
			loginService = _loginService;
        }

        public async Task<IActionResult> Index()
        {
            var x = await inventoryService.Login("temp_user", "temp_password"); //HttpContext.Connection.RemoteIpAddress?.ToString() ?? ""
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
