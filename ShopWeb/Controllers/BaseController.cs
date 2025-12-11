using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? WarningMessage { get; set; }

        [TempData]
        public string? InfoMessage { get; set; }

        protected void SetSuccessMessage(string message)
        {
            SuccessMessage = message;
        }

        protected void SetErrorMessage(string message)
        {
            ErrorMessage = message;
        }

        protected void SetWarningMessage(string message)
        {
            WarningMessage = message;
        }

        protected void SetInfoMessage(string message)
        {
            InfoMessage = message;
        }

        //protected void HandleApiResult<T>(ApiResult<T> result)
        //{
        //    if (!result.IsSuccess)
        //    {
        //        if (result.ValidationErrors != null && result.ValidationErrors.Any())
        //        {
        //            foreach (var error in result.ValidationErrors)
        //            {
        //                foreach (var message in error.Value)
        //                {
        //                    ModelState.AddModelError(error.Key, message);
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(result.ErrorMessage))
        //        {
        //            ModelState.AddModelError(string.Empty, result.ErrorMessage);
        //        }
        //    }
        //}

        //protected void HandleApiResult(ApiResult result)
        //{
        //    if (!result.IsSuccess)
        //    {
        //        if (result.ValidationErrors != null && result.ValidationErrors.Any())
        //        {
        //            foreach (var error in result.ValidationErrors)
        //            {
        //                foreach (var message in error.Value)
        //                {
        //                    ModelState.AddModelError(error.Key, message);
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(result.ErrorMessage))
        //        {
        //            ModelState.AddModelError(string.Empty, result.ErrorMessage);
        //        }
        //    }
        //}

        protected IActionResult ViewWithError(string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return View();
        }

        protected IActionResult RedirectWithSuccess(string action, string message, string? controller = null)
        {
            SetSuccessMessage(message);
            return controller != null
                ? RedirectToAction(action, controller)
                : RedirectToAction(action);
        }

        protected IActionResult RedirectWithError(string action, string message, string? controller = null)
        {
            SetErrorMessage(message);
            return controller != null
                ? RedirectToAction(action, controller)
                : RedirectToAction(action);
        }
    }
}
