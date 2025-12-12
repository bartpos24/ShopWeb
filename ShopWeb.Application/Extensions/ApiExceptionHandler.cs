using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopWeb.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Extensions
{
    public static class ApiExceptionHandler
    {
        /// <summary>
        /// Adds API exception details to ModelState for display on the same page
        /// </summary>
        public static void AddToModelState(ApiException exception, ModelStateDictionary modelState)
        {
            // Add main error message
            modelState.AddModelError(string.Empty, exception.Message);

            // Add validation errors if present
            if (exception.ValidationErrors != null)
            {
                foreach (var error in exception.ValidationErrors)
                {
                    foreach (var message in error.Value)
                    {
                        modelState.AddModelError(error.Key, message);
                    }
                }
            }
        }

        /// <summary>
        /// Adds generic exception message to ModelState
        /// </summary>
        public static void AddToModelState(Exception exception, ModelStateDictionary modelState, string? fallbackMessage = null)
        {
            var message = fallbackMessage ?? "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";

            if (exception is ApiException apiException)
            {
                AddToModelState(apiException, modelState);
            }
            else
            {
                modelState.AddModelError(string.Empty, message);
            }
        }
    }
}
