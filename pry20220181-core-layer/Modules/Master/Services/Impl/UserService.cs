using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<HealthPersonnelDTO> GetHealthPersonnelByUserIdAsync(string userId)
        {
            var healthPersonnelFromDb = await _userRepository.GetHealthPersonnelByUserIdAsync(userId);
            return new HealthPersonnelDTO()
            {
                HealthPersonnelId = healthPersonnelFromDb.HealthPersonnelId,
                DNI = healthPersonnelFromDb.DNI
            };
        }

        public async Task<ParentDTO> GetParentByUserIdAsync(string userId)
        {
            var parentFromDb = await _userRepository.GetParentByUserIdAsync(userId);
            return new ParentDTO()
            {
                ParentId = parentFromDb.ParentId,
                DNI = parentFromDb.DNI
            };
        }
    }
}
