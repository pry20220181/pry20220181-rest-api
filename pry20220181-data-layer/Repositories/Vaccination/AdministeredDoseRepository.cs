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
    public class AdministeredDoseRepository : IAdministeredDoseRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public AdministeredDoseRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AdministeredDose>> GetByChildIdAsync(int childId)
        {
            return await _dbContext.AdministeredDoses
                .Where(x => x.ChildId == childId)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(AdministeredDose administeredDose)
        {
            var registeredAdministeredDose = await _dbContext.AdministeredDoses.AddAsync(administeredDose);
            await _dbContext.SaveChangesAsync();
            return registeredAdministeredDose.Entity.AdministeredDoseId;
        }
    }
}
