
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
				return new ApiErrorResponse { Message = content };
			}
		}
	}
}
