using pry20220181_core_layer.Modules.Campaigns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Repositories
{
    public interface IVaccinationCampaignRepository
    {
        public Task<VaccinationCampaign> GetByIdWithLocationsAndVaccines(int campaignId);
        public Task<List<VaccinationCampaign>> GetByHealthCenterId(int healthCenterId);
    }
}
