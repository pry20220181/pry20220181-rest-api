using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Input
{
    public class VaccinationAppointmentCreationDTO
    {
        public int HealthCenterId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public List<int> VaccinesIds { get; set; }
    }
}
