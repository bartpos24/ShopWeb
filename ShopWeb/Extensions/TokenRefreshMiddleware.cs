using Microsoft.AspNetCore.Authentication;
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
			var publicPaths = new[]
			{
				"/account/login",
				"/account/register",
				"/account/logout",
				"/account/accessdenied",
				"/identity/account/login",
				"/identity/account/register",
				"/identity/account/logout"
			};

			if (publicPaths.Any(p => path.Contains(p)))
			{
				await _next(context);
				return;
			}
			// Check if user is authenticated
			if (!context.User.Identity?.IsAuthenticated ?? true)
			{
				// Not authenticated - let the authentication middleware handle it
				await _next(context);
				return;
			}

			// Check if user has a token
			var token = tokenService.GetAccessToken();

			if (string.IsNullOrEmpty(token))
			{
				// No token but user is authenticated - sign out and redirect
				await context.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
				context.Response.Redirect("/Identity/Account/Login?expired=true");
				return;
			}

			var refreshSuccess = await tokenRefreshService.RefreshTokenIfNeededAsync();
			if (!refreshSuccess)
			{
				// Token expired and refresh failed - sign out and redirect
				tokenService.ClearTokens();
				await context.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
				context.Response.Redirect("/Identity/Account/Login?expired=true");
				return;
			}

			await _next(context);
		}
	}
}
