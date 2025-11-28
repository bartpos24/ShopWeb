using Microsoft.Extensions.DependencyInjection;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Infrastructure.Repositories;

namespace ShopWeb.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
