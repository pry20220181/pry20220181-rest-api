using pry20220181_core_layer.Modules.Campaigns.DTOs.Output;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Campaigns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Campaigns.Services.Impl
{
    public class VaccinationCampaignsService : IVaccinationCampaignsService
    {
        IVaccinationCampaignRepository _vaccinationCampaignRepository { get; set; }

        public VaccinationCampaignsService(IVaccinationCampaignRepository vaccinationCampaignRepository)
        {
            _vaccinationCampaignRepository = vaccinationCampaignRepository;
        }

        public async Task<VaccinationCampaignDetailDTO> GetVaccinationCampaignById(int vaccinationCampaignId)
        {
            var vaccinationCampaignFromDb = await _vaccinationCampaignRepository.GetByIdWithLocationsAndVaccinesAsync(vaccinationCampaignId);
            var vaccinationCampaignToReturn = new VaccinationCampaignDetailDTO()
            {
                VaccinationCampaign = new VaccinationCampaignDTO()
                {
                    VaccinationCampaignId = vaccinationCampaignFromDb.VaccinationCampaignId,
                    Name = vaccinationCampaignFromDb.Name,
                    Description = vaccinationCampaignFromDb.Description,
                    StartDateTime = vaccinationCampaignFromDb.StartDateTime,
                    EndDateTime = vaccinationCampaignFromDb.EndDateTime,
                }
            };

            foreach (var campaignDetail in vaccinationCampaignFromDb.VaccinationCampaignDetails)
            {
                vaccinationCampaignToReturn.Vaccines.Add(new VaccinationCampaignDetailDTO.Vaccine()
                {
                    VaccineId = campaignDetail.Vaccine.VaccineId,
                    Name = campaignDetail.Vaccine.Name,
                    Description = campaignDetail.Vaccine.Description
                });
            }

            foreach (var location in vaccinationCampaignFromDb.VaccinationCampaignLocations)
            {
                vaccinationCampaignToReturn.HealthCenters.Add(new VaccinationCampaignDetailDTO.HealthCenter()
                {
                    HealthCenterId = location.HealthCenterId,
                    Name = location.HealthCenter.Name,
                    Address = location.HealthCenter.Address,
                    UbigeoId = location.HealthCenter.UbigeoId
                });
            }

            return vaccinationCampaignToReturn;
        }

        public async Task<List<VaccinationCampaignDTO>> GetVaccinationCampaignsByHealthCenter(int healthCenterId)
        {
            var vaccinationCampaignsFromDb = await _vaccinationCampaignRepository.GetByHealthCenterIdAsync(healthCenterId);
            var vaccinationCampaignsToReturn = new List<VaccinationCampaignDTO>();
            foreach (var vaccinationCampaign in vaccinationCampaignsFromDb)
            {
                vaccinationCampaignsToReturn.Add(new VaccinationCampaignDTO()
                {
                    VaccinationCampaignId = vaccinationCampaign.VaccinationCampaignId,
                    Name = vaccinationCampaign.Name,
                    Description = vaccinationCampaign.Description,
                    StartDateTime = vaccinationCampaign.StartDateTime,
                    EndDateTime = vaccinationCampaign.EndDateTime,
                });
            }
            return vaccinationCampaignsToReturn;
        }
    }
}
