using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopWeb.Application.Extensions;
using ShopWeb.Domain.Exceptions;

namespace ShopWeb.Pages
{
    public abstract class BasePageModel : PageModel
    {
        /// <summary>
        /// Executes an async action and handles API exceptions by adding them to ModelState
        /// </summary>
        protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (UnauthorizedException)
            {
                // Redirect to login for unauthorized
                return RedirectToPage("/Account/Login", new { returnUrl = Request.Path });
            }
            catch (ForbiddenException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (ApiException ex)
            {
                ApiExceptionHandler.AddToModelState(ex, ModelState);
                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.");
                return Page();
            }
        }

        /// <summary>
        /// Executes an async action and handles exceptions, returns true if successful
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
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.");
                return false;
            }
        }

        /// <summary>
        /// Executes an async action and returns result, handles exceptions
        /// </summary>
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
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.");
                return (false, default);
            }
        }
    }
}
