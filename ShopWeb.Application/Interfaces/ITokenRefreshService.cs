using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Interfaces
{
	public interface ITokenRefreshService
	{
		Task<bool> RefreshTokenIfNeededAsync();
	}
}
