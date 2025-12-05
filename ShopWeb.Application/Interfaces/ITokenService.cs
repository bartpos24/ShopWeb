using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Interfaces
{
	public interface ITokenService
	{
		void StoreTokens(string accessToken, string refreshToken);
		string GetAccessToken();
		string GetRefreshToken();
		DateTime? GetTokenExpirationTime();
		bool IsTokenExpired();
		void ClearTokens();
		ClaimsPrincipal GetClaimsFromToken();
	}
}
