using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class VaccinationAppointmentReminderDTO
    {
        public int ReminderId { get; set; }
        public string Via { get; set; }
        public DateTime SendDate { get; set; }
        public VAReminderParentDTO Parent { get; set; }
        public int VaccinationAppointmentId { get; set; }
        public VAReminderChildDTO Child { get; set; }
        public VAReminderHealthCenterDTO HealthCenter { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public List<string> Vaccines { get; set; }
        public class VAReminderHealthCenterDTO
        {
            public int HealthCenterId { get; set; }
            public string Name { get; set; }
        }
        public class VAReminderParentDTO
        {
            public int ParentId { get; set; }
        }
        public class VAReminderChildDTO
        {
            public int ChildId { get; set; }
            public string Fullname { get; set; }
            public string DNI { get; set; }
        }
    }
}
