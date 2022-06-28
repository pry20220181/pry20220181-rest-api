using pry20220181_core_layer.Modules.Campaigns.DTOs.Input;
using pry20220181_core_layer.Modules.Campaigns.DTOs.Output;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Campaigns.Repositories;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Utils;
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
        IParentRepository _parentRepository { get; set; }
        IReminderRepository _reminderRepository { get; set; }

        public VaccinationCampaignsService(IVaccinationCampaignRepository vaccinationCampaignRepository, IParentRepository parentRepository, IReminderRepository reminderRepository)
        {
            _vaccinationCampaignRepository = vaccinationCampaignRepository;
            _parentRepository = parentRepository;
            _reminderRepository = reminderRepository;
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

        public async Task<int> CreateVaccinationCampaign(VaccinationCampaignCreateDTO vaccinationCampaignCreateDTO)
        {
            var vaccinationCampaignToCreate = new VaccinationCampaign()
            {
                Name = vaccinationCampaignCreateDTO.Name,
                Description = vaccinationCampaignCreateDTO.Description,
                StartDateTime = vaccinationCampaignCreateDTO.StartDateTime,
                EndDateTime = vaccinationCampaignCreateDTO.EndDateTime,
                VaccinationCampaignDetails = new List<VaccinationCampaignDetail>(),
                VaccinationCampaignLocations = new List<VaccinationCampaignLocation>()
            };
            foreach (var healthCenterForCampaign in vaccinationCampaignCreateDTO.CampaignHealthCenters)
            {
                vaccinationCampaignToCreate.VaccinationCampaignLocations.Add(new VaccinationCampaignLocation()
                {
                    HealthCenterId = healthCenterForCampaign.HealthCenterId
                });
            }

            foreach (var vaccineForCampaign in vaccinationCampaignCreateDTO.VaccinesForCampaign)
            {
                vaccinationCampaignToCreate.VaccinationCampaignDetails.Add(new()
                {
                    VaccineId = vaccineForCampaign.VaccineId
                });
            }

            var createdCampaignId = await _vaccinationCampaignRepository.CreateVaccinationCampaign(vaccinationCampaignToCreate);
            var ubigeoIds = await _vaccinationCampaignRepository.GetUbigeosByVaccinationCampaignId(createdCampaignId);
            var parentsAbleToGoTheCampaign = await _parentRepository.GetAllByUbigeoIds(ubigeoIds);
            var remindersToCreate = new List<Reminder>();
            foreach (var parent in parentsAbleToGoTheCampaign)
            {
                Reminder reminder = new Reminder()
                {
                    ParentId = parent.ParentId,
                    SendDate = vaccinationCampaignToCreate.StartDateTime.AddDays(-3),
                    VaccinationCampaignId = createdCampaignId,
                    Via = ReminderVias.SMS
                };
                remindersToCreate.Add(reminder); 
            }
            await _reminderRepository.CreateRangeAsync(remindersToCreate);
            return createdCampaignId;
        }
    }
}
