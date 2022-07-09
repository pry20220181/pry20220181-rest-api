using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services
{
    public interface IVaccinationAppointmentService
    {
        /// <summary>
        /// Create the Vaccination appointment with its related info (What vaccines will be put) and a reminder to notify to the parent when the appointment is near
        /// </summary>
        /// <param name="vaccinationAppointment"></param>
        /// <returns>The Id of the created vaccination appointment</returns>
        public Task<int> CreateVaccinationAppointmentAsync(VaccinationAppointmentCreationDTO vaccinationAppointmentCreationDTO);
    }
}
