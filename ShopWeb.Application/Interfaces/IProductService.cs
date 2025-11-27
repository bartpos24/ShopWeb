using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Interfaces
{
	public interface IProductService
	{
		Task<List<Product>> GetProductByBarcode(string barcode);
	}
}
