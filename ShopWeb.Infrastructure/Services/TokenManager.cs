using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
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
			try
			{
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(GetAccessToken());

                var claims = new List<Claim>();

                // Extract standard claims
                foreach (var claim in jwtToken.Claims)
                {
                    switch (claim.Type)
                    {
						case JwtRegisteredClaimNames.Sub:
							claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
							claims.Add(claim); // Keep original
							break;

						case JwtRegisteredClaimNames.UniqueName:
							claims.Add(new Claim(ClaimTypes.Name, claim.Value));
							claims.Add(claim); // Keep original
							break;

						case JwtRegisteredClaimNames.Email:
							claims.Add(new Claim(ClaimTypes.Email, claim.Value));
							claims.Add(claim); // Keep original
							break;

						// Standard Claims from ClaimTypes
						case ClaimTypes.Name:
							claims.Add(new Claim("given_name", claim.Value));
							claims.Add(claim); // Keep original
							break;

						case ClaimTypes.Surname:
							claims.Add(new Claim("family_name", claim.Value));
							claims.Add(claim); // Keep original
							break;

						case ClaimTypes.Role:
							claims.Add(claim); // Roles can be multiple
							break;

						// Custom Claims
						case "ssaid":
							claims.Add(claim);
							break;

						// ShopLoginClaimsKey (login_type)
						case var _ when claim.Type.Contains("ShopLoginClaimsKey") || claim.Type == "login_type":
							claims.Add(new Claim("login_type", claim.Value));
							claims.Add(claim); // Keep original
							break;

						// JWT Standard Claims (jti, iat, exp, iss, aud)
						case JwtRegisteredClaimNames.Jti:
						case JwtRegisteredClaimNames.Iat:
						case JwtRegisteredClaimNames.Exp:
						case JwtRegisteredClaimNames.Iss:
						case JwtRegisteredClaimNames.Aud:
							claims.Add(claim);
							break;

						default:
							claims.Add(claim);
							break;
					}
                }

                var identity = new ClaimsIdentity(claims, "Bearer");
                return new ClaimsPrincipal(identity);
            }
			catch (Exception ex)
			{
                //logger.LogError(ex, "Error parsing JWT token");
                throw;
            }
		}

        public bool ValidateToken()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
				var token = GetAccessToken();
                if (!tokenHandler.CanReadToken(token))
                    return false;

                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Check if token is expired
                return jwtToken.ValidTo > DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error validating token");
                return false;
            }
        }
        public string? GetClaimValue(string claimType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(GetAccessToken());

                return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error extracting claim from token");
                return null;
            }
        }
    }
}
