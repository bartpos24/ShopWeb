using AutoMapper;
using ShopWeb.Application.TransferObjects.Products;
using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductUnit, ProductUnitVm>();
            CreateMap<ProductUnitVm, ProductUnit>();
        }
    }
}
