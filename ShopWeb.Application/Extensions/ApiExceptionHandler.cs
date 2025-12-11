using Newtonsoft.Json;
using ShopWeb.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Extensions
{
    public class ApiExceptionHandler : IApiExceptionHandler
    {
        //private readonly ILogger<ApiExceptionHandler> _logger;

        //public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
        //{
        //    _logger = logger;
        //}

        //public ApiResult<T> HandleException<T>(Exception ex)
        //{
        //    if (ex is ApiException apiException)
        //    {
        //        return HandleApiException<T>(apiException);
        //    }

        //    // Generic exceptions
        //    _logger.LogError(ex, "Unexpected error occurred");
        //    return ApiResult<T>.Failure(
        //        "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.",
        //        500
        //    );
        //}

        //public async Task<ApiResult<T>> ExecuteAsync<T>(Func<Task<T>> apiCall)
        //{
        //    try
        //    {
        //        var result = await apiCall();
        //        return ApiResult<T>.Success(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException<T>(ex);
        //    }
        //}

        //public async Task<ApiResult> ExecuteAsync(Func<Task> apiCall)
        //{
        //    try
        //    {
        //        await apiCall();
        //        return ApiResult.Success();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is ApiException apiException)
        //        {
        //            return HandleApiException(apiException);
        //        }

        //        _logger.LogError(ex, "Unexpected error occurred");
        //        return ApiResult.Failure(
        //            "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.",
        //            500
        //        );
        //    }
        //}

        //private ApiResult<T> HandleApiException<T>(ApiException apiException)
        //{
        //    var statusCode = apiException.ErrorCode;
        //    var errorMessage = ExtractErrorMessage(apiException);

        //    _logger.LogWarning(
        //        "API Exception: StatusCode={StatusCode}, Message={Message}",
        //        statusCode,
        //        errorMessage
        //    );

        //    // Extract validation errors if present
        //    var validationErrors = ExtractValidationErrors(apiException);

        //    return statusCode switch
        //    {
        //        400 => ApiResult<T>.Failure(errorMessage ?? "Nieprawidłowe żądanie.", 400, validationErrors),
        //        401 => ApiResult<T>.Failure(errorMessage ?? "Brak autoryzacji.", 401),
        //        403 => ApiResult<T>.Failure(errorMessage ?? "Brak dostępu do zasobu.", 403),
        //        404 => ApiResult<T>.Failure(errorMessage ?? "Nie znaleziono zasobu.", 404),
        //        409 => ApiResult<T>.Failure(errorMessage ?? "Konflikt danych.", 409),
        //        422 => ApiResult<T>.Failure(errorMessage ?? "Nieprawidłowe dane walidacji.", 422, validationErrors),
        //        500 => ApiResult<T>.Failure(errorMessage ?? "Błąd serwera.", 500),
        //        _ => ApiResult<T>.Failure(errorMessage ?? "Wystąpił błąd.", statusCode)
        //    };
        //}

        //private ApiResult HandleApiException(ApiException apiException)
        //{
        //    var statusCode = apiException.ErrorCode;
        //    var errorMessage = ExtractErrorMessage(apiException);

        //    _logger.LogWarning(
        //        "API Exception: StatusCode={StatusCode}, Message={Message}",
        //        statusCode,
        //        errorMessage
        //    );

        //    var validationErrors = ExtractValidationErrors(apiException);

        //    return statusCode switch
        //    {
        //        400 => ApiResult.Failure(errorMessage ?? "Nieprawidłowe żądanie.", 400, validationErrors),
        //        401 => ApiResult.Failure(errorMessage ?? "Brak autoryzacji.", 401),
        //        403 => ApiResult.Failure(errorMessage ?? "Brak dostępu do zasobu.", 403),
        //        404 => ApiResult.Failure(errorMessage ?? "Nie znaleziono zasobu.", 404),
        //        409 => ApiResult.Failure(errorMessage ?? "Konflikt danych.", 409),
        //        422 => ApiResult.Failure(errorMessage ?? "Nieprawidłowe dane walidacji.", 422, validationErrors),
        //        500 => ApiResult.Failure(errorMessage ?? "Błąd serwera.", 500),
        //        _ => ApiResult.Failure(errorMessage ?? "Wystąpił błąd.", statusCode)
        //    };
        //}

        //private string? ExtractErrorMessage(ApiException apiException)
        //{
        //    // Try to extract message from ErrorContent
        //    if (apiException.ErrorContent != null)
        //    {
        //        try
        //        {
        //            var errorContentString = apiException.ErrorContent.ToString();

        //            // Try to parse as JSON
        //            if (!string.IsNullOrEmpty(errorContentString))
        //            {
        //                // Handle different error response formats
        //                if (errorContentString.StartsWith("{"))
        //                {
        //                    dynamic errorObj = JsonConvert.DeserializeObject(errorContentString)!;

        //                    // Common patterns in API responses
        //                    if (errorObj.message != null)
        //                        return errorObj.message.ToString();
        //                    if (errorObj.error != null)
        //                        return errorObj.error.ToString();
        //                    if (errorObj.title != null)
        //                        return errorObj.title.ToString();
        //                }
        //                else
        //                {
        //                    // Plain text error message
        //                    return errorContentString;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogDebug(ex, "Could not parse error content");
        //        }
        //    }

        //    // Fallback to exception message
        //    return apiException.Message;
        //}

        //private Dictionary<string, string[]>? ExtractValidationErrors(ApiException apiException)
        //{
        //    if (apiException.ErrorContent == null)
        //        return null;

        //    try
        //    {
        //        var errorContentString = apiException.ErrorContent.ToString();

        //        if (string.IsNullOrEmpty(errorContentString) || !errorContentString.StartsWith("{"))
        //            return null;

        //        dynamic errorObj = JsonConvert.DeserializeObject(errorContentString)!;

        //        // Check for ASP.NET Core validation errors format
        //        if (errorObj.errors != null)
        //        {
        //            var errors = new Dictionary<string, string[]>();
        //            foreach (var error in errorObj.errors)
        //            {
        //                var property = error.Name;
        //                var messages = new List<string>();

        //                foreach (var message in error.Value)
        //                {
        //                    messages.Add(message.ToString());
        //                }

        //                errors[property] = messages.ToArray();
        //            }
        //            return errors;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogDebug(ex, "Could not extract validation errors");
        //    }

        //    return null;
        //}
    }
}
