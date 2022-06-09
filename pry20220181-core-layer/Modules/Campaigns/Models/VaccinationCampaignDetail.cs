using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Models
{
    public class VaccinationCampaignDetail
    {
        public int VaccinationCampaignDetailId { get; set; }
        public int VaccineId { get; set; }
        public int VaccinationCampaignId { get; set; }

        #region Relations with another tables
        public Vaccine Vaccine { get; set; }
        public VaccinationCampaign VaccinationCampaign { get; set; }
        #endregion
    }
}
