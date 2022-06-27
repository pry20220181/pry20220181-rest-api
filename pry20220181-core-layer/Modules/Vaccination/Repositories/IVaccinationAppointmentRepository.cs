﻿using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IVaccinationAppointmentRepository
    {
        public Task<int> CreateAsync(VaccinationAppointment vaccinationAppointment);
    }
}
