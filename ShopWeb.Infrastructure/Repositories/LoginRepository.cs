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
		public LoginRepository(ILoginApi _loginApi)
		{
			loginApi = _loginApi;
		}
		public async Task<string> Login(string username, string password, string ssaid = null)
		{
			var loginModel = new LoginModel
			{
				Username = username,
				Password = password,
				Ssaid = ssaid ?? "",
				LoginType = ELoginType.Web
			};
			return await loginApi.ApiLoginLoginPostAsync(loginModel);
		}
	}
}
