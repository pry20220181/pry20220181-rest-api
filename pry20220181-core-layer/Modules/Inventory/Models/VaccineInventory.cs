using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Inventory.Models
{
    public class VaccineInventory
    {
        public int VaccineInventoryId { get; set; }
        public int VaccineId { get; set; }
        public int HealthCenterId { get; set; }
        public int Stock { get; set; }

        #region Relations with another tables
        public Vaccine Vaccine { get; set; }
        public HealthCenter HealthCenter { get; set; }
        #endregion
    }
}
