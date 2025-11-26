using ShopWeb.Application.Interfaces;
using ShopWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Services
{
	public class LoginService : ILoginService
	{
		private readonly ILoginRepository loginRepository;
		public LoginService(ILoginRepository _loginRepository)
		{
			loginRepository = _loginRepository;
		}
		public async Task<string> Login(string username, string password, string ssaid = null)
		{
			return await loginRepository.Login(username, password, ssaid);
		}
	}
}
