using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.DTOs.Output
{
    public class InventoryDTO
    {
        public int InventoryId { get; set; }
        public int VaccineId { get; set; }
        public int HealthCenterId { get; set; }
        public string VaccineName { get; set; }
        public string HealthCenterName { get; set; }
        public int Stock { get; set; }
    }
}
