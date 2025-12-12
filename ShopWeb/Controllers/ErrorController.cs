using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
	public class ErrorController : Controller
	{
		[Route("Error")]
		public IActionResult Index()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "An unexpected error occurred.";
			ViewBag.ErrorCode = TempData["ErrorCode"];
			ViewBag.TraceId = TempData["ErrorTraceId"];
			return View();
		}

		[Route("Error/NotFound")]
		public IActionResult NotFound()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "The requested resource was not found.";
			Response.StatusCode = 404;
			return View();
		}

		[Route("Error/Forbidden")]
		public IActionResult Forbidden()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "You don't have permission to access this resource.";
			Response.StatusCode = 403;
			return View();
		}

		[Route("Error/ServiceUnavailable")]
		public IActionResult ServiceUnavailable()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "Service is temporarily unavailable.";
			Response.StatusCode = 503;
			return View();
		}
	}
}
