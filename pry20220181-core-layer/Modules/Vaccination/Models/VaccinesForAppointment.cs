using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class VaccineForAppointment
    {
        public int VaccineForAppointmentId { get; set; }
        public int VaccineId { get; set; }
        public int VaccinationAppointmentId { get; set; }

        #region Relations with another tables
        public Vaccine Vaccine { get; set; }
        public VaccinationAppointment VaccinationAppointment { get; set; }
        #endregion
    }
}
