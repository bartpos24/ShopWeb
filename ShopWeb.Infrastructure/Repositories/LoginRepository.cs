using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.Repositories
{
	public class LoginRepository : ILoginRepository
	{
		private readonly ILoginApi loginApi;
		private readonly IHttpContextAccessor httpContextAccessor;
		public LoginRepository(ILoginApi _loginApi, IHttpContextAccessor _httpContextAccessor)
		{
			loginApi = _loginApi;
			httpContextAccessor = _httpContextAccessor;
		}
		public async Task<string> Login(string username, string password, string ssaid = null)
		{
			var loginModel = new LoginModel(username, password, ssaid, ELoginType.Web);
			var token = await loginApi.ApiLoginLoginPostAsync(loginModel);
			if (!string.IsNullOrEmpty(token))
			{
				httpContextAccessor.HttpContext?.Session.SetString("JWTAccessSecretKey", token);
			}
			return token;
		}
	}
}
