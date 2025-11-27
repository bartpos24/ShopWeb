using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Interfaces
{
	public interface IProductRepository
	{
		Task<List<Product>> GetProductByBarcode(string barcode);
	}
}
