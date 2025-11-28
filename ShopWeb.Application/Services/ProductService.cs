using ShopWeb.Application.Interfaces;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;

namespace ShopWeb.Application.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository productRepository;
		public ProductService(IProductRepository _productRepository)
		{
			productRepository = _productRepository;
		}
		public async Task<List<Product>> GetProductByBarcode(string barcode)
		{
			return await productRepository.GetProductByBarcode(barcode);
		}
	}
}
