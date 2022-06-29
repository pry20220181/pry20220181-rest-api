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
        public int ParentId { get; set; }
        public int DoseDetailId { get; set; }
        public int ChildId { get; set; }
    }
}
