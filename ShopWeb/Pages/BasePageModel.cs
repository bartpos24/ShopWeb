using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopWeb.Pages
{
    public abstract class BasePageModel : PageModel
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

        protected IActionResult PageWithError(string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return Page();
        }

        protected IActionResult RedirectWithSuccess(string page, string message)
        {
            SetSuccessMessage(message);
            return RedirectToPage(page);
        }

        protected IActionResult RedirectWithError(string page, string message)
        {
            SetErrorMessage(message);
            return RedirectToPage(page);
        }
    }
}
