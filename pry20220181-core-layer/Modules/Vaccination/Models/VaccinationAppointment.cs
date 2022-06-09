using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class VaccinationAppointment
    {
        public int VaccinationAppointmentId { get; set; }
        public int HealthCenterId { get; set; }
        public int ParentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }

        #region Relations with another tables
        public HealthCenter HealthCenter { get; set; }
        public List<VaccineForAppointment> VaccinesForAppointment { get; set; }
        #endregion
    }
}
