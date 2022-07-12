using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IDoseDetailRepository
    {
        public Task<List<DoseDetail>> CreateRangeAsync(List<DoseDetail> dosesDetails);
        /// <summary>
        /// Get the Doses with the info abouts its Scheme and Vaccine
        /// </summary>
        /// <returns>List of Doses with its Scheme and Vaccine info</returns>
        public Task<List<DoseDetail>> GetAllWithSchemesAndVaccinesAsync();
        public Task<VaccinationScheme> GetVaccinationSchemeByDoseDetailIdAsync(int doseDetailId);
        
    }
}
