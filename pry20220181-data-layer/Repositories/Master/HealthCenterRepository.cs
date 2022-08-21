using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class HealthCenterRepository : IHealthCenterRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public HealthCenterRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HealthCenter>> GetHealthCentersByUbigeoIdsAsync(List<int> ubigeoIds)
        {
            return await _dbContext.HealthCenters
                .Where(h => ubigeoIds.Contains(h.UbigeoId))
                .ToListAsync();
        }

        public async Task<HealthCenter> GetHealthCenterById(int healthCenterId)
        {
            return await _dbContext.HealthCenters
                .FirstOrDefaultAsync(h => h.HealthCenterId == healthCenterId);
        }

        public async Task<List<HealthCenter>> GetHealthCenters()
        {
            return await _dbContext.HealthCenters
                .ToListAsync();
        }
    }
}
