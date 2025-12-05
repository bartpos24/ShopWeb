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
        private readonly ILoginService loginService;
        private readonly IProductService productService;

		public HomeController(ILogger<HomeController> _logger, ILoginService _loginService, IProductService _productService)
		{
			logger = _logger;
			loginService = _loginService;
			productService = _productService;
		}

		public async Task<IActionResult> Index()
        {
            try
            {
				var product = await productService.GetProductByBarcode("5449000034519");
				var c = product;
			} catch (Exception ex)
            {
                if(ex is ApiException)
                {
                    ex = ex as ApiException;
                    return View(new ErrorViewModel { RequestId = $"Wyst¹pi³ b³¹d: {ex.Message}" });
				}
                logger.LogError(ex, "Error in HomeController Index");
			}
			
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
