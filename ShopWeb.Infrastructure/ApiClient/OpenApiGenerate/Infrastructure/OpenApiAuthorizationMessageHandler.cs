using Microsoft.AspNetCore.Http;
using ShopWeb.Domain.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure
{
	public class OpenApiAuthorizationMessageHandler : DelegatingHandler
	{
		private readonly ITokenManager tokenManager;

		public OpenApiAuthorizationMessageHandler(ITokenManager _tokenManager)
		{
			tokenManager = _tokenManager;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// Get token from session
			var token = tokenManager.GetAccessToken();

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}




