using pry20220181_core_layer.Modules.Campaigns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.DTOs.Input
{
    public class VaccinationCampaignCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        #region Relations with another tables
        public List<VaccineForCreateCampaign> VaccinesForCampaign { get; set; } = new List<VaccineForCreateCampaign>();
        public List<CampaignHealthCenter> CampaignHealthCenters { get; set; } = new List<CampaignHealthCenter>();
        #endregion
        public class VaccineForCreateCampaign
        {
            public int VaccineId { get; set; }
        }
        public class CampaignHealthCenter
        {
            public int HealthCenterId { get; set; }
        }
    }
}
