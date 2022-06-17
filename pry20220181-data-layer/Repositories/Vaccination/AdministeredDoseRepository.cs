using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Vaccination
{
    public class AdministeredDoseRepository : IAdministeredDoseRepository
    {
        public Task<List<AdministeredDose>> GetByChild(int childId)
        {
            throw new NotImplementedException();
        }
    }
}
