using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Interfaces
{
	public interface ILoginService
	{
		Task<string> Login(string username, string password, string? ssaid = null);
		Task<string> RefreshToken(string refreshToken, string? ssaid = null);
		Task Logout();
	}
}
