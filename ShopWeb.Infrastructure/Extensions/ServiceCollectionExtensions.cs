using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Infrastructure.ApiClient.NSwag;
using ShopWeb.Infrastructure.ApiClient.NSwag.Infrastructure;
using ShopWeb.Infrastructure.ApiClient.NSwag.Models;
using ShopWeb.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get API base URL from configuration
            var apiBaseUrl = configuration["ShopApi:BaseUrl"]
                ?? throw new ArgumentNullException("ShopApi:BaseUrl is not configured");

            // Register ShopApi Client
            services.AddHttpClient<IShopApiNSwagClient, ShopApiNSwagClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddTypedClient<IShopApiNSwagClient>((httpClient, serviceProvider) =>
            {
                return new ShopApiNSwagClient(apiBaseUrl, httpClient);
            });

            //// Register Login Client
            //services.AddHttpClient<ILoginClient, LoginClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //});

            //// Register Product Client
            //services.AddHttpClient<IProductClient, ProductClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //});

            //// Register User Client
            //services.AddHttpClient<IUserClient, UserClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //});

            // Register custom services/handlers
            services.AddTransient<AuthorizationMessageHandler>();

            services.AddScoped<IInventoryRepository, InventoryRepository>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServicesWithAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // First add the basic services
            services.AddInfrastructureServices(configuration);

            // Get API base URL from configuration
            var apiBaseUrl = configuration["ShopApi:BaseUrl"]
                ?? throw new ArgumentNullException("ShopApi:BaseUrl is not configured");

            // Re-register clients with authentication handler
            services.AddHttpClient<IShopApiNSwagClient, ShopApiNSwagClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .AddTypedClient<IShopApiNSwagClient>((httpClient, serviceProvider) =>
            {
                return new ShopApiNSwagClient(apiBaseUrl, httpClient);
            });

            //// Re-register clients with authentication handler
            //services.AddHttpClient<ILoginClient, LoginClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //})
            //.AddHttpMessageHandler<AuthorizationMessageHandler>();

            //services.AddHttpClient<IProductClient, ProductClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //})
            //.AddHttpMessageHandler<AuthorizationMessageHandler>();

            //services.AddHttpClient<IUserClient, UserClient>(client =>
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //})
            //.AddHttpMessageHandler<AuthorizationMessageHandler>();

            // Register custom services/handlers
            services.AddTransient<AuthorizationMessageHandler>();

            services.AddScoped<IInventoryRepository, InventoryRepository>();

            return services;
        }
    }
}
