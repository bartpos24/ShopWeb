using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;
using ShopWeb.Infrastructure.ApiClient.OpenApiGenerate.Api;

namespace ShopWeb.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly IProductApi productApi;
		public ProductRepository(IProductApi _productApi)
		{
			productApi = _productApi;
		}

		public async Task<List<Product>> GetProductByBarcode(string barcode)
		{
			return await productApi.ApiProductGetProductByBarcodeGetAsync(barcode);
		}
		public async Task<List<ProductUnit>> GetAllUnits()
		{
			return await productApi.ApiProductGetAllUnitsGetAsync();
        }
    }
}
