using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.DTOs.Input
{
    public class AddVaccineStockDTO
    {
        public int InventoryId { get; set; }
        public int StockToAdd { get; set; }
    }
}
