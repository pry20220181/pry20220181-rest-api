using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class DoseReminderDTO
    {
        public int ReminderId { get; set; }
        public string Via { get; set; }
        public DateTime SendDate { get; set; }
        public DoseReminderParentDTO Parent { get; set; }
        public DoseReminderChildDTO Child { get; set; }
        public DoseDTO Dose { get; set; }
        public class DoseReminderChildDTO
        {
            public int ChildId { get; set; }
            public string DNI { get; set; }
            public string Name { get; set; }
        }
        public class DoseReminderParentDTO
        {
            public int ParentId { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
        }
        public class DoseDTO {
            public int DoseDetailId { get; set; }
            public string VaccineName { get; set; }
            public int DoseNumber {get;set;}
        }
    }
}
