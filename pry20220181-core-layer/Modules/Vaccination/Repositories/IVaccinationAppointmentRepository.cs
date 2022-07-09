using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IVaccinationAppointmentRepository
    {
        /// <summary>
        /// Create the Vaccination appointment with its related info (What vaccines will be put)
        /// </summary>
        /// <param name="vaccinationAppointment"></param>
        /// <returns>The Id of the created vaccination appointment</returns>
        public Task<int> CreateAsync(VaccinationAppointment vaccinationAppointment);
    }
}
