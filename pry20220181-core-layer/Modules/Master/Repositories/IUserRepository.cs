using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IUserRepository
    {
        public Task<Parent> GetParentByUserIdAsync(string userId);
        public Task<HealthPersonnel> GetHealthPersonnelByUserIdAsync(string userId);
    }
}
