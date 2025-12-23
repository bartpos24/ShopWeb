using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class SummaryInventoryPositionVm
    {
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public DateTime ScanDate { get; set; }
    }
}
