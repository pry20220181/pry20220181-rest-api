using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class HealthCenter
    {
        public int HealthCenterId { get; set; }
        public int UbigeoId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        #region Relations with another tables
        public Ubigeo Ubigeo { get; set; }
        public List<VaccinationCampaignLocation> VaccinationCampaignLocations { get; set; }
        public List<VaccineInventory> VaccineInventories { get; set; }
        #endregion
    }
}
