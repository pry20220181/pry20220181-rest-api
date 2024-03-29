﻿using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Repositories
{
    public interface IVaccinationCampaignRepository
    {
        public Task<VaccinationCampaign> GetByIdWithLocationsAndVaccinesAsync(int campaignId);
        public Task<List<VaccinationCampaign>> GetByHealthCenterIdAsync(int healthCenterId, DateTime sinceDate);
        public Task<int> CreateVaccinationCampaign(VaccinationCampaign vaccinationCampaign);
        public Task<List<int>> GetUbigeosByVaccinationCampaignId(int vaccinationCampaignId);
    }
}
