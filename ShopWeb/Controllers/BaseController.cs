using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Application.Extensions;
using ShopWeb.Domain.Exceptions;

namespace ShopWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Executes an action and handles API exceptions
        /// </summary>
        protected async Task<IActionResult> ExecuteAsync<TModel>(
            Func<Task<IActionResult>> action,
            TModel model) where TModel : class
        {
            try
            {
                return await action();
            }
            catch (UnauthorizedException)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            catch (ApiException ex)
            {
                ApiExceptionHandler.AddToModelState(ex, ModelState);
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd.");
                return View(model);
            }
        }

        /// <summary>
        /// Try execute and return success status
        /// </summary>
        protected async Task<bool> TryExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
                return true;
            }
            catch (ApiException ex)
            {
                ApiExceptionHandler.AddToModelState(ex, ModelState);
                return false;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd.");
                return false;
            }
        }

        protected async Task<(bool Success, T? Result)> TryExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return (true, result);
            }
            catch (ApiException ex)
            {
                ApiExceptionHandler.AddToModelState(ex, ModelState);
                return (false, default);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.");
                return (false, default);
            }
        }
    }
}
