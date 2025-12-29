using AutoMapper;
using ShopWeb.Application.TransferObjects.User;
using ShopWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterModelVm, RegisterModel>();
        }
    }
}
