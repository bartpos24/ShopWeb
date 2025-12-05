using Microsoft.Extensions.Logging;
using ShopWeb.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Services
{
	public class TokenRefreshService : ITokenRefreshService
	{
		private readonly ITokenService tokenService;
		private readonly ILoginService loginService;
		private readonly ILogger<TokenRefreshService> logger;

		public TokenRefreshService(ITokenService _tokenService, ILoginService _loginService, ILogger<TokenRefreshService> _logger)
		{
			tokenService = _tokenService;
			loginService = _loginService;
			logger = _logger;
		}

		public async Task<bool> RefreshTokenIfNeededAsync()
		{
			try
			{
				// Check if token is expired or about to expire
				if (!tokenService.IsTokenExpired())
				{
					return true; // Token is still valid
				}

				var refreshToken = tokenService.GetAccessToken();//tokenService.GetRefreshToken();
				if (string.IsNullOrEmpty(refreshToken))
				{
					logger.LogWarning("No refresh token available. User needs to re-login.");
					tokenService.ClearTokens();
					return false;
				}

				// Call refresh endpoint
				var newAccessToken = await loginService.RefreshToken(refreshToken);

				if (string.IsNullOrEmpty(newAccessToken))
				{
					logger.LogWarning("Token refresh failed. Clearing tokens.");
					tokenService.ClearTokens();
					return false;
				}

				// Store new tokens (refresh token might also be updated)
				tokenService.StoreTokens(newAccessToken, refreshToken);
				logger.LogInformation("Token refreshed successfully.");

				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during token refresh");
				tokenService.ClearTokens();
				return false;
			}
		}
	}
}
