using Microsoft.AspNetCore.Http;
using ShopWeb.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopWeb.Infrastructure.Services
{
	public class TokenManager : ITokenManager
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private const string AccessTokenKey = "JWTAccessToken";
		private const string RefreshTokenKey = "JWTRefreshToken";
		private const string TokenExpiryKey = "JWTTokenExpiry";

		public TokenManager(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void StoreTokens(string accessToken, string refreshToken)
		{
			var session = _httpContextAccessor.HttpContext?.Session;
			if (session == null) return;

			session.SetString(AccessTokenKey, accessToken);

			if (!string.IsNullOrEmpty(refreshToken))
			{
				session.SetString(RefreshTokenKey, refreshToken);
			}

			// Extract expiration time from JWT token
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(accessToken);
			var expiry = jwtToken.ValidTo;

			session.SetString(TokenExpiryKey, expiry.ToString("o"));
		}

		public string GetAccessToken()
		{
			return _httpContextAccessor.HttpContext?.Session.GetString(AccessTokenKey);
		}

		public string GetRefreshToken()
		{
			return _httpContextAccessor.HttpContext?.Session.GetString(RefreshTokenKey);
		}

		public DateTime? GetTokenExpirationTime()
		{
			var expiryString = _httpContextAccessor.HttpContext?.Session.GetString(TokenExpiryKey);
			if (string.IsNullOrEmpty(expiryString)) return null;

			return DateTime.Parse(expiryString);
		}

		public bool IsTokenExpired()
		{
			var expiry = GetTokenExpirationTime();
			if (!expiry.HasValue) return true;

			// Check if token expires in the next 2 minutes (buffer time)
			return expiry.Value.AddMinutes(-2) <= DateTime.UtcNow;
		}

		public void ClearTokens()
		{
			var session = _httpContextAccessor.HttpContext?.Session;
			session?.Remove(AccessTokenKey);
			session?.Remove(RefreshTokenKey);
			session?.Remove(TokenExpiryKey);
		}

		public ClaimsPrincipal GetClaimsFromToken()
		{
			var token = GetAccessToken();
			if (string.IsNullOrEmpty(token)) return null;

			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
			return new ClaimsPrincipal(identity);
		}
	}
}
