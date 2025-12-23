using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Application.TransferObjects.Inventory
{
    public class InventorySummaryVm
    {
        public List<SummaryInventoryPositionVm> Positions { get; set; } = new List<SummaryInventoryPositionVm>();
        public InventoryVm Inventory { get; set; } = new InventoryVm();
        public int Count { get; set; }
    }
}
