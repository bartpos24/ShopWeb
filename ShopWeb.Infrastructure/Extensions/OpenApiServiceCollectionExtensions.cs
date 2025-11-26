using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Infrastructure;
using System;

namespace ShopWeb.Infrastructure.Extensions
{
	public static class OpenApiServiceCollectionExtensions
	{
		public static IServiceCollection AddOpenApiGeneratedClients(this IServiceCollection services, IConfiguration configuration)
		{
			// Get API base URL from configuration
			var apiBaseUrl = configuration["ShopApi:BaseUrl"]
				?? throw new ArgumentNullException("ShopApi:BaseUrl is not configured");

			// Register named HttpClient for OpenAPI generated clients
			services.AddHttpClient("OpenApiShopClient", client =>
			{
				client.BaseAddress = new Uri(apiBaseUrl);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
				client.Timeout = TimeSpan.FromSeconds(30);
			});

			// Register Configuration for OpenAPI clients
			services.AddSingleton<IReadableConfiguration>(sp =>
			{
				return new Configuration
				{
					BasePath = apiBaseUrl
				};
			});

			// Register API clients
			services.AddScoped<ILoginApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new LoginApi(httpClient, config);
			});

			services.AddScoped<IProductApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new ProductApi(httpClient, config);
			});

			services.AddScoped<IUserApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new UserApi(httpClient, config);
			});

			return services;
		}

		public static IServiceCollection AddOpenApiGeneratedClientsWithAuth(this IServiceCollection services, IConfiguration configuration)
		{
			// Get API base URL from configuration
			var apiBaseUrl = configuration["ShopApi:BaseUrl"]
				?? throw new ArgumentNullException("ShopApi:BaseUrl is not configured");

			// Register authentication message handler
			services.AddTransient<OpenApiAuthorizationMessageHandler>();

			// Register named HttpClient with authentication handler
			services.AddHttpClient("OpenApiShopClient", client =>
			{
				client.BaseAddress = new Uri(apiBaseUrl);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
				client.Timeout = TimeSpan.FromSeconds(30);
			})
			.AddHttpMessageHandler<OpenApiAuthorizationMessageHandler>();

			// Register Configuration for OpenAPI clients
			services.AddSingleton<IReadableConfiguration>(sp =>
			{
				return new Configuration
				{
					BasePath = apiBaseUrl
				};
			});

			// Register API clients - the OpenApiAuthorizationMessageHandler will inject the token at request time
			services.AddScoped<ILoginApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new LoginApi(httpClient, config);
			});

			services.AddScoped<IProductApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new ProductApi(httpClient, config);
			});

			services.AddScoped<IUserApi>(sp =>
			{
				var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenApiShopClient");
				var config = new Configuration { BasePath = apiBaseUrl };
				return new UserApi(httpClient, config);
			});

			return services;
		}
	}
}