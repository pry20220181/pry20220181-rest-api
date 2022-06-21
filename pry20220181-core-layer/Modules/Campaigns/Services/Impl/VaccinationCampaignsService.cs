using pry20220181_core_layer.Modules.Campaigns.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Services.Impl
{
    public class VaccinationCampaignsService : IVaccinationCampaignsService
    {
        public Task<VaccinationCampaignDetailDTO> GetVaccinationCampaignById(int vaccinationCampaignId)
        {
            throw new NotImplementedException();
        }

        public Task<List<VaccinationCampaignDTO>> GetVaccinationCampaignsByHealthCenter(int healthCenterId)
        {
            throw new NotImplementedException();
        }
    }
}
