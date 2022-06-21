using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IVaccinationSchemeDetailRepository
    {
        public Task<List<VaccinationSchemeDetail>> CreateRangeAsync(List<VaccinationSchemeDetail> vaccinationSchemeDetails);
        public Task<List<VaccinationSchemeDetail>> GetByVaccineIdAsync(int vaccineId);
        public Task<List<VaccinationSchemeDetail>> GetAllWithVaccinesAndDosesAsync();
    }
}
