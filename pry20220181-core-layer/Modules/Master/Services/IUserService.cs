using pry20220181_core_layer.Modules.Master.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services
{
    public interface IUserService
    {
        public Task<ParentDTO> GetParentByUserIdAsync(string userId);
        public Task<HealthPersonnelDTO> GetHealthPersonnelByUserIdAsync(string userId);
    }
}
