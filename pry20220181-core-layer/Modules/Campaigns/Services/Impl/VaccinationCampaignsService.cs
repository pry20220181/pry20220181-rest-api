using Microsoft.Extensions.Logging;
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
        ILogger<VaccinationCampaignsService> _logger { get; set; }

        public VaccinationCampaignsService(IVaccinationCampaignRepository vaccinationCampaignRepository, IParentRepository parentRepository, IReminderRepository reminderRepository, ILogger<VaccinationCampaignsService> logger)
        {
            _vaccinationCampaignRepository = vaccinationCampaignRepository;
            _parentRepository = parentRepository;
            _reminderRepository = reminderRepository;
            _logger = logger;
        }

        public async Task<VaccinationCampaignDetailDTO> GetVaccinationCampaignById(int vaccinationCampaignId)
        {
            if(vaccinationCampaignId < 1)
            {
                return null;
            }

            var vaccinationCampaignFromDb = await _vaccinationCampaignRepository.GetByIdWithLocationsAndVaccinesAsync(vaccinationCampaignId);

            if(vaccinationCampaignFromDb is null)
            {
                _logger.LogInformation($"The Vaccination Campaign with ID {vaccinationCampaignId} does not exist");
                return null;
            }

            var vaccinationCampaignToReturn = new VaccinationCampaignDetailDTO()
            {
                VaccinationCampaignId = vaccinationCampaignFromDb.VaccinationCampaignId,
                Name = vaccinationCampaignFromDb.Name,
                Description = vaccinationCampaignFromDb.Description,
                StartDateTime = vaccinationCampaignFromDb.StartDateTime,
                EndDateTime = vaccinationCampaignFromDb.EndDateTime
            };

            vaccinationCampaignToReturn.Vaccines = vaccinationCampaignFromDb.VaccinationCampaignDetails
                .Select(campaignDetail => new VaccinationCampaignDetailDTO.VaccineForCampaign()
                {
                    VaccineId = campaignDetail.Vaccine.VaccineId,
                    Name = campaignDetail.Vaccine.Name,
                    Description = campaignDetail.Vaccine.Description
                }).ToList();

            vaccinationCampaignToReturn.HealthCenters = vaccinationCampaignFromDb.VaccinationCampaignLocations
                .Select(location => new VaccinationCampaignDetailDTO.HealthCenter()
                {
                    HealthCenterId = location.HealthCenterId,
                    Name = location.HealthCenter.Name,
                    Address = location.HealthCenter.Address,
                    UbigeoId = location.HealthCenter.UbigeoId
                }).ToList();

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
            if(vaccinationCampaignCreateDTO is null)
            {
                return 0;
            }

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
            
            if(createdCampaignId > 0)
            {
                _logger.LogInformation($"A Vaccination campaign with ID {createdCampaignId} was created");
                #region Create the reminders for the parents that could go to the campaing
                var ubigeoIds = await _vaccinationCampaignRepository.GetUbigeosByVaccinationCampaignId(createdCampaignId);

                if (!(ubigeoIds is null))
                {
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
                    _logger.LogInformation($"{remindersToCreate.Count()} reminders were created for this Vaccination Campaign");
                }
                #endregion
            }

            return createdCampaignId;
        }
    }
}
