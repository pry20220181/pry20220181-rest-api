using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IHealthPersonnelRepository
    {
        /// <summary>
        /// Create the HealthPersonnel record in the Database
        /// </summary>
        /// <param name="healthPersonnel"></param>
        /// <returns>The id of the new HealthPersonnel</returns>
        public Task<int> CreateAsync(HealthPersonnel healthPersonnel);
        public Task<HealthPersonnel> GetByIdAsync(int healthPersonnelId);
    }
}
