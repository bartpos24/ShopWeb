using ShopWeb.Application.TransferObjects;
using System.Security.Claims;

namespace ShopWeb.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static UserInfoVm GetUserInfo(this ClaimsPrincipal user)
		{
			if (!user.Identity?.IsAuthenticated ?? true)
				return null;

			return new UserInfoVm
			{
				UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
				Username = user.FindFirst(ClaimTypes.Name)?.Value,
				GivenName = user.FindFirst("given_name")?.Value,
				FamilyName = user.FindFirst("family_name")?.Value,
				Email = user.FindFirst(ClaimTypes.Email)?.Value,
				Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
				LoginType = user.FindFirst("login_type")?.Value,
				IpAddress = user.FindFirst("ssaid")?.Value
			};
		}
	}
}
