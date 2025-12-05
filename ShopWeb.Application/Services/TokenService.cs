using ShopWeb.Application.Interfaces;
using ShopWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Services
{
	public class TokenService : ITokenService
	{
		private readonly ITokenManager tokenManager;

		public TokenService(ITokenManager _tokenManager)
		{
			tokenManager = _tokenManager;
		}

		public void ClearTokens()
		{
			tokenManager.ClearTokens();
		}

		public string GetAccessToken()
		{
			return tokenManager.GetAccessToken();
		}

		public ClaimsPrincipal GetClaimsFromToken()
		{
			return tokenManager.GetClaimsFromToken();
		}

		public string GetRefreshToken()
		{
			return tokenManager.GetRefreshToken();
		}

		public DateTime? GetTokenExpirationTime()
		{
			return tokenManager.GetTokenExpirationTime();
		}

		public bool IsTokenExpired()
		{
			return tokenManager.IsTokenExpired();
		}

		public void StoreTokens(string accessToken, string refreshToken)
		{
			tokenManager.StoreTokens(accessToken, refreshToken);
		}
	}
}
