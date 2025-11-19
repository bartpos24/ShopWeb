using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.ApiClient.NSwag
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        //private readonly IHttpContextAccessor httpContextAccessor;

        //public AuthorizationMessageHandler(IHttpContextAccessor _httpContextAccessor)
        //{
        //    this.httpContextAccessor = _httpContextAccessor;
        //}
        private readonly IConfiguration configuration;

        public AuthorizationMessageHandler(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get token from session, cookie, or wherever you store it
            var token = configuration["Jwt:JWTAccessSecretKey"];//httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
