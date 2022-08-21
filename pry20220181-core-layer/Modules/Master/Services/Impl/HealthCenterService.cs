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
    public class HealthCenterService : IHealthCenterService
    {
        private IHealthCenterRepository _healthCenterRepository { get; set; }
        ILogger<HealthCenterService> _logger { get; set; }

        public HealthCenterService(IHealthCenterRepository healthCenterRepository, ILogger<HealthCenterService> logger)
        {
            _healthCenterRepository = healthCenterRepository;
            _logger = logger;
        }

        public async Task<List<HealthCenterDTO>> GetHealthCentersByUbigeosAsync(List<int> UbigeoIds)
        {
            var healthCentersFromDB = await _healthCenterRepository.GetHealthCentersByUbigeoIdsAsync(UbigeoIds);

            return healthCentersFromDB.Select(h => new HealthCenterDTO
            {
                HealthCenterId = h.HealthCenterId,
                Name = h.Name,
                Address = h.Address,
                UbigeoId = h.UbigeoId
            }).ToList();
        }

        public async Task<HealthCenterDTO> GetHealthCenterById(int healthCenterId)
        {
            var healthCenterFromDb = await _healthCenterRepository.GetHealthCenterById(healthCenterId);

            HealthCenterDTO healthCenter = new HealthCenterDTO()
            {
                HealthCenterId = healthCenterFromDb.HealthCenterId,
                Name = healthCenterFromDb.Name,
                UbigeoId = healthCenterFromDb.UbigeoId,
                Address = healthCenterFromDb.Address
            };

            return healthCenter;
        }

        public async Task<List<HealthCenterDTO>> GetHealthCenters()
        {
            var healthCentersFromDb = await _healthCenterRepository.GetHealthCenters();

            return healthCentersFromDb.Select(h=> new HealthCenterDTO(){
                HealthCenterId = h.HealthCenterId,
                Name = h.Name,
                UbigeoId = h.UbigeoId,
                Address = h.Address
            }).ToList();
        }
    }
}
