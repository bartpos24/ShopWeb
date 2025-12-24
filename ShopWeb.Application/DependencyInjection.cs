using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Mapping;
using ShopWeb.Application.Services;
using System.Reflection;

namespace ShopWeb.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenRefreshService, TokenRefreshService>();
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<InventoryMapping>();
                cfg.AddProfile<ProductMapping>();

                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
            }, Assembly.GetExecutingAssembly());
			return services;
        }
    }
}
