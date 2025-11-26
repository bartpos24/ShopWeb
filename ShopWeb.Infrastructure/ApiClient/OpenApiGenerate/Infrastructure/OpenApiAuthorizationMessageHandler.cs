using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure
{
	public class OpenApiAuthorizationMessageHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public OpenApiAuthorizationMessageHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			// Get token from session
			var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTAccessSecretKey");

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}




