using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Interfaces
{
	public interface ILoginRepository
	{
		Task<string> Login(string username, string password, string? ssaid = null);
		Task<string> RefreshToken(string refreshToken, string? ssaid = null);
		Task Logout();
		Task Register(RegisterModel registerModel);

    }
}
