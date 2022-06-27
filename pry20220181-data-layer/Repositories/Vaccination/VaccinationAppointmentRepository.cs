using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Vaccination
{
    public class VaccinationAppointmentRepository : IVaccinationAppointmentRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public VaccinationAppointmentRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(VaccinationAppointment vaccinationAppointment)
        {
            var createdAppointmnet = await _dbContext.VaccinationAppointments.AddAsync(vaccinationAppointment);
            await _dbContext.SaveChangesAsync();
            return createdAppointmnet.Entity.VaccinationAppointmentId;
        }
    }
}
