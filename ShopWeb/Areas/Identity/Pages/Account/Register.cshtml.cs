// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using ShopWeb.Application.TransferObjects.User;
using ShopWeb.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace ShopWeb.Areas.Identity.Pages.Account
{
    public class RegisterModel : BasePageModel
    {
        private readonly ILogger<RegisterModel> logger;
        private readonly ILoginService loginService;

        public RegisterModel(ILogger<RegisterModel> _logger, ILoginService _loginService)
        {
            logger = _logger;
            loginService = _loginService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public RegisterModelVm Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var registerModel = new RegisterModelVm()
            {
                Name = Input.Name,
                Surname = Input.Surname,
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password,
                ConfirmPassword = Input.ConfirmPassword
            };
            try
            {
                await loginService.Register(registerModel);
                return LocalRedirect(returnUrl);
            } catch(Exception ex)
            {
                logger.LogError(ex, "Error registering user {Username}", Input.Username);
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd podczas rejestracji. Spróbuj ponownie później.");
                return Page();
            }
        }
    }
}
