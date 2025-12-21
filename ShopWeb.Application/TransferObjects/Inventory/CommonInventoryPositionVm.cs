using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class CommonInventoryPositionVm
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public DateTime ScanDate { get; set; }
        public int UserId { get; set; }
        public int InventoryId { get; set; }
        public int? ModifiedByUserId { get; set; }
        public int UnitId { get; set; }
    }
}
