// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShopWeb.Application.Interfaces;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWeb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        //private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly ILoginService loginService;
        private readonly ITokenService tokenService;

        public LoginModel(ILogger<LoginModel> _logger, ILoginService _loginService, ITokenService _tokenService)
        {
            //signInManager = _signInManager;
            logger = _logger;
            loginService = _loginService;
            tokenService = _tokenService;
		}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string Login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

			if (Request.Query.ContainsKey("expired"))
			{
				ModelState.AddModelError(string.Empty, "Your session has expired. Please log in again.");
			}

			returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                try
                {
					var ssaid = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
					var token = await loginService.Login(Input.Login, Input.Password, ssaid);
					if (!string.IsNullOrEmpty(token))
					{
						logger.LogInformation("User {Username} logged in successfully.", Input.Login);
						return LocalRedirect(returnUrl);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
						return Page();
					}
				} catch (Exception ex)
                {
					if (ex is ApiException)
					{
						ex = ex as ApiException;
                        var x = ex;
					}
					logger.LogError(ex, "Error during login for user {Username}", Input.Login);
					ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
					return Page();
				}
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
