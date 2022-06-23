using pry20220181_core_layer.Modules.Campaigns.DTOs.Input;
using pry20220181_core_layer.Modules.Campaigns.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Services
{
    public interface IVaccinationCampaignsService
    {
        public Task<List<VaccinationCampaignDTO>> GetVaccinationCampaignsByHealthCenter(int healthCenterId);
        public Task<VaccinationCampaignDetailDTO> GetVaccinationCampaignById(int vaccinationCampaignId);
        public Task<int> CreateVaccinationCampaign(VaccinationCampaignCreateDTO vaccinationCampaignCreateDTO);
    }
}
