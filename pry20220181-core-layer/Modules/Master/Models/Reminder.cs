using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Models
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public string Via { get; set; }
        public DateTime SendDate { get; set; }
        public int ParentId { get; set; }
        public int VaccinationCampaignId { get; set; }
        public int VaccinationAppointmentId { get; set; }
        public int DoseDetailId { get; set; }
        public int ChildId { get; set; }

        #region Relations with another tables
        public Parent Parent { get; set; }
        public Child Child { get; set; }
        public VaccinationCampaign VaccinationCampaign { get; set; }
        public VaccinationAppointment VaccinationAppointment { get; set; }
        public DoseDetail DoseDetail { get; set; }
        #endregion
    }
}
