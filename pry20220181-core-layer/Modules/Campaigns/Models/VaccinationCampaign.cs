using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Models
{
    public class VaccinationCampaign
    {
        public int VaccinationCampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        #region Relations with another tables
        public List<VaccinationCampaignDetail> VaccinationCampaignDetails { get; set; }
        public List<VaccinationCampaignLocation> VaccinationCampaignLocations { get; set; }
        #endregion
    }
}
