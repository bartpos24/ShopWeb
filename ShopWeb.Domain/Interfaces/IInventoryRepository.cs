using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<string> Login(string username, string password, string ssaid = null);
    }
}
