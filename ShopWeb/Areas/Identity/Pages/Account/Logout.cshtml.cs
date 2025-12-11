// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using ShopWeb.Pages;
using System;
using System.Threading.Tasks;

namespace ShopWeb.Areas.Identity.Pages.Account
{
    public class LogoutModel : BasePageModel
    {
        private readonly ILogger<LogoutModel> logger;
        private readonly ITokenService tokenService;
        private readonly ILoginService loginService;

		public LogoutModel(ILogger<LogoutModel> _logger, ITokenService _tokenService, ILoginService _loginService)
        {
            logger = _logger;
            tokenService = _tokenService;
            loginService = _loginService;
		}

		public async Task<IActionResult> OnGet(string returnUrl = null)
		{
			return await PerformLogout(returnUrl);
		}

		public async Task<IActionResult> OnPost(string returnUrl = null)
		{
			return await PerformLogout(returnUrl);
		}

		private async Task<IActionResult> PerformLogout(string returnUrl)
		{
			var userName = User.Identity?.Name ?? "Unknown";

			try
			{
				// Call API logout endpoint (optional - if your API tracks sessions)
				await loginService.Logout();
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Error calling API logout endpoint for user {UserName}", userName);
				// Continue with local logout even if API call fails
			}

			// Sign out from cookie authentication
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			logger.LogInformation("User {UserName} logged out successfully", userName);

			if (!string.IsNullOrEmpty(returnUrl))
			{
				return LocalRedirect(returnUrl);
			}

			return RedirectToPage("/Account/Login", new { area = "Identity", loggedOut = true });
		}

		//public async Task<IActionResult> OnPost(string returnUrl = null)
		//      {
		//          var userName = User.Identity?.Name ?? "Unknown";
		//          await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		//          logger.LogInformation("User {UserName} logged out", userName);

		//          if (returnUrl != null)
		//          {
		//              return LocalRedirect(returnUrl);
		//          }
		//          return RedirectToPage("/Index");
		//          //await _signInManager.SignOutAsync();
		//          //_logger.LogInformation("User logged out.");
		//          //if (returnUrl != null)
		//          //{
		//          //    return LocalRedirect(returnUrl);
		//          //}
		//          //else
		//          //{
		//          //    // This needs to be a redirect so that the browser performs a new
		//          //    // request and the identity for the user gets updated.
		//          //    return RedirectToPage();
		//          //}
		//      }
	}
}
