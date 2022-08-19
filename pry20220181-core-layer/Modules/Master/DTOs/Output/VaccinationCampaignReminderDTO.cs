using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class VaccinationCampaignReminderDTO
    {
        public int ReminderId { get; set; }
        public string Via { get; set; }
        public DateTime SendDate { get; set; }
        public VaccinationCampaignReminderParent Parent { get; set; }
        public VaccinationCampaignPayload VaccinationCampaign { get; set; }

        public class VaccinationCampaignPayload
        {
            public int VaccinationCampaignId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public List<VaccinationCampaignReminderHealthCenter> HealthCenters { get; set; }
            public List<string> Vaccines { get; set; }
        }
        public class VaccinationCampaignReminderParent
        {
            public int ParentId { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
        }
        public class VaccinationCampaignReminderHealthCenter
        {
            public int HealthCenterId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}
