using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Vaccination
{
    public class VaccinationSchemeDetailRepository : IVaccinationSchemeDetailRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public VaccinationSchemeDetailRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VaccinationSchemeDetail>> CreateRangeAsync(List<VaccinationSchemeDetail> vaccinationSchemeDetails)
        {
            _dbContext.VaccinationSchemeDetails.AddRange(vaccinationSchemeDetails);
            await _dbContext.SaveChangesAsync();
            return vaccinationSchemeDetails;
        }
    }
}
