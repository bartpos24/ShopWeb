using ShopWeb.Domain.Interfaces;
using ShopWeb.Infrastructure.ApiClient.NSwag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IShopApiNSwagClient shopApi;
        public InventoryRepository(IShopApiNSwagClient _shopApiClient)
        {
            shopApi = _shopApiClient;
        }
        public async Task<string> Login(string username, string password, string ssaid = null)
        {
            var loginModel = new LoginModel
            {
                Username = username,
                Password = password,
                Ssaid = ssaid,
                LoginType = ELoginType.Web
            };
            return await shopApi.LoginAsync(loginModel);
        }
    }
}
