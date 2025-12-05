using ShopWeb.Application.Interfaces;

namespace ShopWeb.Extensions
{
	public class TokenRefreshMiddleware
	{
		private readonly RequestDelegate _next;

		public TokenRefreshMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ITokenRefreshService tokenRefreshService, ITokenService tokenService)
		{
			// Skip token check for login/register pages
			var path = context.Request.Path.Value?.ToLower();
			if (path.Contains("/account/login") || path.Contains("/account/register") || path.Contains("/account/logout"))
			{
				await _next(context);
				return;
			}

			// Check if user has a token
			var token = tokenService.GetAccessToken();
			if (!string.IsNullOrEmpty(token))
			{
				// Try to refresh if needed
				var refreshSuccess = await tokenRefreshService.RefreshTokenIfNeededAsync();

				if (!refreshSuccess)
				{
					// Token expired and refresh failed - redirect to login
					context.Response.Redirect("/Identity/Account/Login?expired=true");
					return;
				}
			}

			await _next(context);
		}
	}
}
