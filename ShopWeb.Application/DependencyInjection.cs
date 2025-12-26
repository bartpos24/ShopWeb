using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.Mapping;
using ShopWeb.Application.Services;
using ShopWeb.Application.TransferObjects.Inventory;
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
                cfg.Advanced.AllowAdditiveTypeMapCreation = true;
            }, Assembly.GetExecutingAssembly());
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();


            return services;
        }
    }
}
