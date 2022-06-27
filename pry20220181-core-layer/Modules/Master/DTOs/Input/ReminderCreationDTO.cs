using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Input
{
    public class ReminderCreationDTO
    {
        public string Via { get; set; }
        public DateTime SendDate { get; set; }
        public int ParentId { get; set; }
        public int VaccinationCampaignId { get; set; } = 0;
        public int VaccinationAppointmentId { get; set; } = 0;
        public int DoseDetailId { get; set; } = 0;
    }
}
