using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class HealthPersonnelService : IHealthPersonnelService
    {
        private IHealthPersonnelRepository _healthPersonnelRepository { get; set; }

        public HealthPersonnelService(IHealthPersonnelRepository healthPersonnelRepository)
        {
            _healthPersonnelRepository = healthPersonnelRepository;
        }

        public async Task<int> RegisterHealthPersonnelAsync(HealthPersonnelCreateDTO healthPersonnelCreateDTO)
        {
            HealthPersonnel healthPersonnel = new HealthPersonnel()
            {
                UserId = healthPersonnelCreateDTO.UserId
            };

            return await _healthPersonnelRepository.CreateAsync(healthPersonnel);
        }
    }
}
