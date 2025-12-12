using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopWeb.Domain.Exceptions;
using System.Diagnostics;

namespace ShopWeb.Services
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

			// Log the exception
			_logger.LogError(exception,
				"An error occurred. TraceId: {TraceId}, Path: {Path}",
				traceId, context.Request.Path);

			// Check if this is an AJAX request
			if (IsAjaxRequest(context))
			{
				await HandleAjaxExceptionAsync(context, exception, traceId);
				return;
			}

			// Handle MVC/Razor Pages requests
			await HandleMvcExceptionAsync(context, exception, traceId);
		}

		private bool IsAjaxRequest(HttpContext context)
		{
			return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
				   || context.Request.Headers.Accept.ToString().Contains("application/json");
		}

		private async Task HandleAjaxExceptionAsync(HttpContext context, Exception exception, string traceId)
		{
			context.Response.ContentType = "application/json";

			var (statusCode, message, errors) = GetExceptionDetails(exception);
			context.Response.StatusCode = statusCode;

			var response = new
			{
				success = false,
				message,
				errors,
				traceId
			};

			await context.Response.WriteAsJsonAsync(response);
		}

		private async Task HandleMvcExceptionAsync(HttpContext context, Exception exception, string traceId)
		{
			var (statusCode, message, errors) = GetExceptionDetails(exception);

			// Store error info in TempData for display
			var tempDataFactory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
			var tempData = tempDataFactory.GetTempData(context);

			tempData["ErrorMessage"] = message;
			tempData["ErrorCode"] = (exception as ApiException)?.ErrorCode;
			tempData["ErrorTraceId"] = traceId;
			tempData["ValidationErrors"] = errors != null
				? System.Text.Json.JsonSerializer.Serialize(errors)
				: null;

			// Redirect based on status code
			var redirectPath = statusCode switch
			{
				401 => "/Account/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path),
				403 => "/Error/Forbidden",
				404 => "/Error/NotFound",
				503 => "/Error/ServiceUnavailable",
				_ => "/Error"
			};

			context.Response.Redirect(redirectPath);
			await Task.CompletedTask;
		}

		private (int StatusCode, string Message, IDictionary<string, string[]>? Errors) GetExceptionDetails(Exception exception)
		{
			return exception switch
			{
				BadRequestException badRequest => (
					400,
					badRequest.Message,
					badRequest.ValidationErrors),

				UnauthorizedException unauthorized => (
					401,
					unauthorized.Message,
					null),

				ForbiddenException forbidden => (
					403,
					forbidden.Message,
					null),

				NotFoundException notFound => (
					404,
					notFound.Message,
					null),

				ConflictException conflict => (
					409,
					conflict.Message,
					null),

				ServiceUnavailableException serviceUnavailable => (
					503,
					serviceUnavailable.Message,
					null),

				ApiException apiException => (
					apiException.StatusCode,
					apiException.Message,
					apiException.ValidationErrors),

				_ => (
					500,
					"An unexpected error occurred. Please try again later.",
					null)
			};
		}
	}
}
