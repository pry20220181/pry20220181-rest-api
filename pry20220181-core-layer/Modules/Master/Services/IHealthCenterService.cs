﻿using pry20220181_core_layer.Modules.Master.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services
{
    public interface IHealthCenterService
    {
        public Task<List<HealthCenterDTO>> GetHealthCenters();
        public Task<HealthCenterDTO> GetHealthCenterById(int healthCenterId);
        public Task<List<HealthCenterDTO>> GetHealthCentersByUbigeosAsync(List<int> UbigeoIds);
    }
}
