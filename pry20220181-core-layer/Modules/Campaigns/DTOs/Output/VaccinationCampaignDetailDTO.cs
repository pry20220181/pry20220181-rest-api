using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.DTOs.Output
{
    public class VaccinationCampaignDetailDTO
    {
        public int VaccinationCampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<VaccineForCampaign> Vaccines { get; set; } = new List<VaccineForCampaign>();
        public List<HealthCenter> HealthCenters { get; set; } = new List<HealthCenter>();
        public class VaccineForCampaign
        {
            public int VaccineId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class HealthCenter
        {
            public int HealthCenterId { get; set; }
            public int UbigeoId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }

}
