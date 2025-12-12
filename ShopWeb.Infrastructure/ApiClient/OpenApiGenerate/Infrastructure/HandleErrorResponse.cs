
using ShopWeb.Domain.Exceptions;
using ShopWeb.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure
{
	public static class HandleErrorResponse
	{
		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		public static void HandleError(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			var errorContent = response.Content.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult();
			var apiError = TryParseApiError(errorContent);

			throw response.StatusCode switch
			{
				HttpStatusCode.BadRequest => new BadRequestException(
					apiError?.Message ?? "Invalid request.",
					apiError?.Errors ?? new Dictionary<string, string[]>()),

				HttpStatusCode.Unauthorized => new UnauthorizedException(
					apiError?.Message ?? "Authentication required."),

				HttpStatusCode.Forbidden => new ForbiddenException(
					apiError?.Message ?? "Access denied."),

				HttpStatusCode.NotFound => new NotFoundException(
					apiError?.Message ?? "Resource not found."),

				HttpStatusCode.Conflict => new ConflictException(
					apiError?.Message ?? "Resource conflict."),

				HttpStatusCode.InternalServerError => new ShopWeb.Domain.Exceptions.ApiException(
					apiError?.Message ?? "An internal server error occurred.", 500),

				HttpStatusCode.ServiceUnavailable => new ServiceUnavailableException(
					apiError?.Message ?? "Service temporarily unavailable."),

				_ => new ShopWeb.Domain.Exceptions.ApiException(
					apiError?.Message ?? $"API request failed with status code {(int)response.StatusCode}",
					(int)response.StatusCode)
			};
		}

		private static ApiErrorResponse? TryParseApiError(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
				return null;

			try
			{
				return JsonSerializer.Deserialize<ApiErrorResponse>(content, _jsonOptions);
			}
			catch
			{
                try
                {
                    // Try to deserialize as a plain JSON string (which removes the escaped quotes)
                    var plainMessage = JsonSerializer.Deserialize<string>(content, _jsonOptions);
                    if (!string.IsNullOrWhiteSpace(plainMessage))
                    {
                        return new ApiErrorResponse { Message = plainMessage };
                    }
                }
                catch
                {
                    // If all else fails, return the raw content
                    // But remove surrounding quotes if present
                    var cleanedContent = content.Trim();
                    if (cleanedContent.StartsWith("\"") && cleanedContent.EndsWith("\""))
                    {
                        cleanedContent = cleanedContent.Substring(1, cleanedContent.Length - 2);
                        // Unescape any escaped characters
                        cleanedContent = cleanedContent
                            .Replace("\\\"", "\"")
                            .Replace("\\\\", "\\")
                            .Replace("\\n", "\n")
                            .Replace("\\r", "\r")
                            .Replace("\\t", "\t");
                    }
                    return new ApiErrorResponse { Message = cleanedContent };
                }

                return new ApiErrorResponse { Message = content };
            }
		}
	}
}
