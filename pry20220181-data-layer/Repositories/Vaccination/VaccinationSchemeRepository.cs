using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Vaccination
{
    public class VaccinationSchemeRepository : IVaccinationSchemeRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public VaccinationSchemeRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VaccinationScheme>> CreateRangeAsync(List<VaccinationScheme> vaccinationSchemes)
        {
            _dbContext.VaccinationSchemes.AddRange(vaccinationSchemes);
            await _dbContext.SaveChangesAsync();
            return vaccinationSchemes;
        }

        public async Task<List<VaccinationScheme>> GetAllAsync()
        {
            return await _dbContext.VaccinationSchemes.ToListAsync();
        }
    }
}
