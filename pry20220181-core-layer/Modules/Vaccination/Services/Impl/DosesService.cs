﻿using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services.Impl
{
    public class DosesService : IDosesService
    {
        private readonly IDoseDetailRepository _doseDetailRepository;
        private readonly IAdministeredDoseRepository _administeredDoseRepository;
        private readonly IChildRepository _childRepository;
        private readonly IReminderRepository _reminderRepository;
        private readonly ILogger<DosesService> _logger;

        public DosesService(IDoseDetailRepository dosesRepository, IAdministeredDoseRepository administeredDoseRepository, IChildRepository childRepository, IReminderRepository reminderRepository, ILogger<DosesService> logger)
        {
            _doseDetailRepository = dosesRepository;
            _administeredDoseRepository = administeredDoseRepository;
            _childRepository = childRepository;
            _reminderRepository = reminderRepository;
            _logger = logger;
        }

        public async Task<List<RemainingDoseDTO>> GetRemainingDosesByChild(int childId)
        {
            if (childId < 1)
            {
                return null;
            }
            var allDosesFromDb = await _doseDetailRepository.GetAllWithSchemesAndVaccinesAsync();
            var administeredDosesToChild = await _administeredDoseRepository.GetByChildIdAsync(childId);
            var child = await _childRepository.GetByIdAsync(childId);

            if (child is null)
            {
                return null;
            }

            var remainingDosesToAdminister = allDosesFromDb
                .Where(d => administeredDosesToChild
                .Exists(ad => ad.DoseDetailId == d.DoseDetailId) == false)
                .ToList();

            remainingDosesToAdminister = DosesAnalyzer.EvaluateIfTheDosesCanBePut(child, administeredDosesToChild, remainingDosesToAdminister);

            var remainingDosesToAdministerToReturn = new List<RemainingDoseDTO>();
            foreach (var remainingDose in remainingDosesToAdminister)
            {
                RemainingDoseDTO remainingDoseDTO = new RemainingDoseDTO()
                {
                    RemainingDoseId = remainingDose.DoseDetailId,
                    VaccinationSchemeDetailId = remainingDose.VaccinationSchemeDetailId,
                    VaccinationSchemeId = remainingDose.VaccinationSchemeDetail.VaccinationSchemeId,
                    DoseNumber = remainingDose.DoseNumber,
                    PutWhen = WhenPutVaccine.ToString(remainingDose),
                    PossibleEffectsPostVaccine = remainingDose.VaccinationSchemeDetail.PossibleEffectsPostVaccine,
                    VaccinationSchemeName = remainingDose.VaccinationSchemeDetail.VaccinationScheme.Name,
                    VaccinationSchemeInitialAge = remainingDose.VaccinationSchemeDetail.VaccinationScheme.InitialAge,
                    VaccinationSchemeFinalAge = remainingDose.VaccinationSchemeDetail.VaccinationScheme.FinalAge,
                    VaccineId = remainingDose.VaccinationSchemeDetail.VaccineId,
                    VaccineName = remainingDose.VaccinationSchemeDetail.Vaccine.Name,
                    CanBePut = remainingDose.CanBePut
                };
                remainingDosesToAdministerToReturn.Add(remainingDoseDTO);
            }

            return remainingDosesToAdministerToReturn;
        }

        public async Task<string> CreateAdministeredDose(AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            #region Register the administered dose
            int childId = administeredDoseCreationDTO.ChildId;
            int doseDetailid = administeredDoseCreationDTO.DoseDetailId;
            var administeredDose = new AdministeredDose()
            {
                DoseDetailId = doseDetailid,
                ChildId = childId,
                HealthCenterId = administeredDoseCreationDTO.HealthCenterId,
                HealthPersonnelId = administeredDoseCreationDTO.HealthPersonnelId,
                DoseDate = administeredDoseCreationDTO.DoseDate,
                Observations = administeredDoseCreationDTO.Observations
            };
            var adminesteredDoseId = await _administeredDoseRepository.CreateAsync(administeredDose);
            _logger.LogInformation($"Administered dose {adminesteredDoseId} created (DoseDetailId: {doseDetailid}, ChildId: {childId})");
            #endregion

            #region Delete the reminder for this DoseDetail and Child if exists
            await _reminderRepository.DeleteRemindersByDoseDetailAndChildIdAsync(doseDetailid, childId);
            #endregion

            #region Get the remaining doses of the current Scheme that are able to put
            var currentVaccinationScheme = await _doseDetailRepository.GetVaccinationSchemeByDoseDetailIdAsync(doseDetailid);
            var currentVaccinationSchemeDetails = currentVaccinationScheme.VaccinationSchemeDetails;
            var currentDosesDetails = new List<DoseDetail>();
            var dosesDetailIds = new List<int>();

            foreach (var vaccinationSchemeDetail in currentVaccinationSchemeDetails)
            {
                currentDosesDetails.AddRange(vaccinationSchemeDetail.DosesDetails);
                dosesDetailIds.AddRange(vaccinationSchemeDetail.DosesDetails.Select(d => d.DoseDetailId));
            }

            var administeredDosesOfCurrentVaccinationScheme = await _administeredDoseRepository.GetByDosesIdList(childId, dosesDetailIds);

            var child = await _childRepository.GetByIdWithParentsIdAsync(childId);
            var remainingDosesToAdminister = currentDosesDetails
                .Where(d => administeredDosesOfCurrentVaccinationScheme
                .Exists(ad => ad.DoseDetailId == d.DoseDetailId) == false)
                .ToList();

            remainingDosesToAdminister = DosesAnalyzer.EvaluateIfTheDosesCanBePut(child, administeredDosesOfCurrentVaccinationScheme, remainingDosesToAdminister);
            var remainingDosesToAdministerThatCanBePut = remainingDosesToAdminister.Where(d => d.CanBePut).ToList();
            _logger.LogInformation($"For the child {childId} are {remainingDosesToAdminister.Count} remaining doses to administer. {remainingDosesToAdministerThatCanBePut.Count} doses are apt to administer");
            #endregion

            #region Create the reminders for the remaining doses that are apt to put
            if (remainingDosesToAdministerThatCanBePut.Count > 0)
            {
                var parentId = child.ChildParents[0].ParentId;
                var remindersToCreate = new List<Reminder>();
                var remindersForThisParent = await _reminderRepository.GetAllDoseRemindersByParentIdAsync(parentId);

                foreach (var remainingDose in remainingDosesToAdministerThatCanBePut)
                {
                    if (remindersForThisParent.Exists(r => r.DoseDetailId == remainingDose.DoseDetailId && r.ChildId == childId))
                    {//This for dont create a reminder of a doseDetailId-ChildId Pair more than once
                        continue;
                    }
                    remindersToCreate.Add(new Reminder()
                    {
                        ChildId = childId,
                        DoseDetailId = remainingDose.DoseDetailId,
                        ParentId = parentId,
                        SendDate = DateTime.UtcNow.AddHours(-5).AddDays(7),
                        Via = ReminderVias.SMS
                    });

                    #region For testing purposes create a reminder for today
                    Reminder reminderToday = new Reminder()
                    {
                        ChildId = childId,
                        DoseDetailId = remainingDose.DoseDetailId,
                        ParentId = parentId,
                        SendDate = DateTime.Now,
                        Via = ReminderVias.SMS
                    };
                    remindersToCreate.Add(reminderToday);
                    #endregion
                }
                await _reminderRepository.CreateRangeAsync(remindersToCreate);
                _logger.LogInformation($"{remindersToCreate.Count} reminders has been created for the parent {parentId} of their child about their reamining doses apt to administer");
            }
            #endregion
            //O SEA TENGO QUE CREAR UN RECORDATORIO PARA TODAS ESTAS DOSIS RESTANTES QUE YA SE PUEDEN PONER,
            //LO QUE FALTA ES EL CUANDO ENVIO ESTE RECORDATORIO: 28/6/22 22:26 Para no loquearme mucho, los pongo para dentro de una semana

            //AHORA PARA UN ESCENARIO MAS REAL HABRIA UN JOB QUE REVISE DE TODOS LOS NIÑOS (por departamento x ejemplo)
            //QUE VACUNAS RESTANTES ESTAN APTAS PARA QUE SE LES PONGA, Y DE ESAS CREAR LOS REMINDERS

            return adminesteredDoseId;
        }

        public async Task<List<AdministeredDoseDTO>> GetAdministeredDosesByChild(int childId)
        {
            if (childId < 1)
            {
                return null;
            }

            var administeredDosesToChildFromDb = await _administeredDoseRepository.GetByChildIdWithAllRelatedInfoAsync(childId);
            var administeredDosesToChildToReturn = new List<AdministeredDoseDTO>();
            _logger.LogInformation($"The Child with ID {childId} has {administeredDosesToChildFromDb.Count()} administered doses");

            foreach (var administeredDose in administeredDosesToChildFromDb)
            {

                AdministeredDoseDTO administeredDoseDTO = new AdministeredDoseDTO()
                {
                    AdministeredDoseId = administeredDose.AdministeredDoseId,
                    VaccineName = administeredDose.DoseDetail.VaccinationSchemeDetail.Vaccine.Name,
                    VaccineId = administeredDose.DoseDetail.VaccinationSchemeDetail.Vaccine.VaccineId,
                    DoseNumber = administeredDose.DoseDetail.DoseNumber,
                    AdministrationDate = administeredDose.DoseDate,
                    HealthCenterName = administeredDose.HealthCenter.Name,
                    HealthPersonnelName = administeredDose.HealthPersonnel.User.FirstName + " " + administeredDose.HealthPersonnel.User.LastName,
                    VaccinationSchemeName = administeredDose.DoseDetail.VaccinationSchemeDetail.VaccinationScheme.Name,
                    WhenShouldBePut = WhenPutVaccine.ToString(administeredDose.DoseDetail),
                    VaccinationSchemeDetailId = administeredDose.DoseDetail.VaccinationSchemeDetailId,
                    VaccinationSchemeId = administeredDose.DoseDetail.VaccinationSchemeDetail.VaccinationSchemeId,
                    DoseId = administeredDose.DoseDetailId,
                    Observations = administeredDose.Observations
                };

                administeredDosesToChildToReturn.Add(administeredDoseDTO);
            }

            return administeredDosesToChildToReturn;
        }

        public async Task<AdministeredDoseDTO> GetAdministeredDoseByIdAsync(string administeredDoseId)
        {
            var administeredDose = await _administeredDoseRepository.GetAdministeredDoseByIdAsync(administeredDoseId);

            return new AdministeredDoseDTO()
            {
                AdministeredDoseId = administeredDose.AdministeredDoseId,
                VaccineName = administeredDose.DoseDetail.VaccinationSchemeDetail.Vaccine.Name,
                VaccineId = administeredDose.DoseDetail.VaccinationSchemeDetail.Vaccine.VaccineId,
                DoseNumber = administeredDose.DoseDetail.DoseNumber,
                AdministrationDate = administeredDose.DoseDate,
                HealthCenterName = administeredDose.HealthCenter.Name,
                HealthPersonnelName = administeredDose.HealthPersonnel.User.FirstName + " " + administeredDose.HealthPersonnel.User.LastName,
                VaccinationSchemeName = administeredDose.DoseDetail.VaccinationSchemeDetail.VaccinationScheme.Name,
                WhenShouldBePut = WhenPutVaccine.ToString(administeredDose.DoseDetail),
                VaccinationSchemeDetailId = administeredDose.DoseDetail.VaccinationSchemeDetailId,
                VaccinationSchemeId = administeredDose.DoseDetail.VaccinationSchemeDetail.VaccinationSchemeId,
                DoseId = administeredDose.DoseDetailId,
                Observations = administeredDose.Observations
            };
        }
    }
}
