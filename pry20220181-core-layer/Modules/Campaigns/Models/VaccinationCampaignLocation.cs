using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Models
{
    public class VaccinationCampaignLocation
    {
        public int VaccinationCampaignLocationId { get; set; }
        public int VaccinationCampaignId { get; set; }
        public int HealthCenterId { get; set; }

        #region Relations with another tables
        public VaccinationCampaign VaccinationCampaign { get; set; }
        public HealthCenter HealthCenter { get; set; }
        #endregion
    }
}
