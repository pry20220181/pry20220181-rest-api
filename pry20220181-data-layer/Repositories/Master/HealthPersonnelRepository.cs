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
    public class HealthPersonnelRepository : IHealthPersonnelRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public HealthPersonnelRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(HealthPersonnel healthPersonnel)
        {
            if (healthPersonnel.UserId is null)
            {
                throw new NotImplementedException("The userId field is not present");
            }

            if ((await _dbContext.HealthPersonnel.FirstOrDefaultAsync(p => p.DNI == healthPersonnel.DNI)) is null)
            {
                var createdHealthPersonnel = await _dbContext.HealthPersonnel.AddAsync(healthPersonnel);

                await _dbContext.SaveChangesAsync();

                return createdHealthPersonnel.Entity.HealthPersonnelId;
            }
            else
            {
                throw new Exception($"A Health Personnel with DNI {healthPersonnel.DNI} already exists");
            }

        }
    }
}
