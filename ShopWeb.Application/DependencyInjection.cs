using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Services;
using System.Reflection;

namespace ShopWeb.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
