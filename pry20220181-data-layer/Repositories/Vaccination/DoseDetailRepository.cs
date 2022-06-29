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
    public class DoseDetailRepository : IDoseDetailRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public DoseDetailRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DoseDetail>> CreateRangeAsync(List<DoseDetail> dosesDetails)
        {
            _dbContext.DosesDetails.AddRange(dosesDetails);
            await _dbContext.SaveChangesAsync();
            return dosesDetails;
        }

        public async Task<List<DoseDetail>> GetAllWithSchemesAndVaccinesAsync()
        {
            return await _dbContext.DosesDetails
                .Include(d => d.VaccinationSchemeDetail)
                    .ThenInclude(v => v.VaccinationScheme)
                .Include(d => d.VaccinationSchemeDetail)
                    .ThenInclude(v => v.Vaccine)
                .ToListAsync();
        }

        public async Task<VaccinationScheme> GetVaccinationSchemeByDoseDetailIdAsync(int doseDetailId)
        {
            var vaccinationSchemeId = await _dbContext.DosesDetails
                .Include(d => d.VaccinationSchemeDetail)
                    .ThenInclude(s=>s.DosesDetails)
                .Include(d => d.VaccinationSchemeDetail)
                    .ThenInclude(s => s.VaccinationScheme)
                .Include(d => d.VaccinationSchemeDetail)
                    .ThenInclude(s => s.Vaccine)
                .Where(d => d.DoseDetailId == doseDetailId)
                .FirstOrDefaultAsync();

            return vaccinationSchemeId.VaccinationSchemeDetail.VaccinationScheme;
        }
    }
}
