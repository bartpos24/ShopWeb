using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Interfaces;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IProductService productService;

		public HomeController(ILogger<HomeController> _logger, IProductService _productService)
		{
			logger = _logger;
			productService = _productService;
		}

		public async Task<IActionResult> Index()
        {
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
