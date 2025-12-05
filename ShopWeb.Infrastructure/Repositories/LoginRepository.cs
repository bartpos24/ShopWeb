using Microsoft.AspNetCore.Http;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api;

namespace ShopWeb.Infrastructure.Repositories
{
	public class LoginRepository : ILoginRepository
	{
		private readonly ILoginApi loginApi;
		private readonly ITokenManager tokenManager;
		public LoginRepository(ILoginApi _loginApi, ITokenManager _tokenManager)
		{
			loginApi = _loginApi;
			tokenManager = _tokenManager;
		}
		public async Task<string> Login(string username, string password, string? ssaid = null)
		{
			var loginModel = new LoginModel(username, password, ssaid, ELoginType.Web);
			var token = await loginApi.ApiLoginLoginPostAsync(loginModel);
			if (!string.IsNullOrEmpty(token))
			{
				tokenManager.StoreTokens(token, null);
			}
			return token;
		}
		public async Task<string> RefreshToken(string refreshToken, string? ssaid = null)
		{
			var newAccessToken = await loginApi.ApiLoginRefreshPostAsync(ssaid, refreshToken);
			return newAccessToken;
		}
		public async Task Logout()
		{
			try
			{
				await loginApi.ApiLoginLogoutPostAsync();
			}
			finally
			{
				tokenManager.ClearTokens();
			}
		}

		
	}
}
