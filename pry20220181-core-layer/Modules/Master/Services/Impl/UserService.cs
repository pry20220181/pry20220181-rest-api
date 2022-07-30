using pry20220181_core_layer.Modules.Master.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class UserService : IUserService
    {
        public Task<HealthPersonnelDTO> GetHealthPersonnelByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ParentDTO> GetParentByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
