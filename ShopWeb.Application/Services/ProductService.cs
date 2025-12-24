using AutoMapper;
using ShopWeb.Application.Interfaces;
using ShopWeb.Application.TransferObjects.Inventory;
using ShopWeb.Application.TransferObjects.Products;
using ShopWeb.Domain.Interfaces;
using ShopWeb.Domain.Models;

namespace ShopWeb.Application.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        public ProductService(IProductRepository _productRepository, IMapper _mapper)
		{
			productRepository = _productRepository;
			mapper = _mapper;
        }
		public async Task<List<Product>> GetProductByBarcode(string barcode)
		{
			return await productRepository.GetProductByBarcode(barcode);
		}
		public async Task<List<ProductUnitVm>> GetAllUnits()
		{
			var units = await productRepository.GetAllUnits();
            return mapper.Map<List<ProductUnitVm>>(units);
        }
    }
}
