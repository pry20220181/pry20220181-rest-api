using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
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
        ILogger<HealthPersonnelService> _logger { get; set; }

        public HealthPersonnelService(IHealthPersonnelRepository healthPersonnelRepository, ILogger<HealthPersonnelService> logger)
        {
            _healthPersonnelRepository = healthPersonnelRepository;
            _logger = logger;
        }

        public async Task<int> RegisterHealthPersonnelAsync(HealthPersonnelCreateDTO healthPersonnelCreateDTO)
        {
            if (healthPersonnelCreateDTO.DNI is null)
            {
                return 0;
            }

            HealthPersonnel healthPersonnel = new HealthPersonnel()
            {
                UserId = healthPersonnelCreateDTO.UserId,
                DNI = healthPersonnelCreateDTO.DNI
            };
            var createHealthPersonnelId = await _healthPersonnelRepository.CreateAsync(healthPersonnel);

            _logger.LogInformation($"The health personnel with ID {createHealthPersonnelId} and DNI {healthPersonnel.DNI} was created.");

            return createHealthPersonnelId;
        }

        public async Task<HealthPersonnelDTO> GetHealthPersonnelAsync(int healthPersonnelId)
        {
            var healthPersonnelFromDb = await _healthPersonnelRepository.GetByIdAsync(healthPersonnelId);
            return new HealthPersonnelDTO()
            {
                DNI = healthPersonnelFromDb.DNI,
                Email = healthPersonnelFromDb.User.Email,
                FirstName = healthPersonnelFromDb.User.FirstName,
                LastName = healthPersonnelFromDb.User.LastName
            };
        }
    }
}
