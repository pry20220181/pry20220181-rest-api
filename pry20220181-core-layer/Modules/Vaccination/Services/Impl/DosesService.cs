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

        public DosesService(IDoseDetailRepository dosesRepository, IAdministeredDoseRepository administeredDoseRepository)
        {
            _doseDetailRepository = dosesRepository;
            _administeredDoseRepository = administeredDoseRepository;
        }

        public async Task<List<RemainingDoseDTO>> GetRemainingDosesByChild(int childId)
        {
            var allDosesFromDb = await _doseDetailRepository.GetAllWithSchemesAndVaccinesAsync();
            var administeredDosesToChild = await _administeredDoseRepository.GetByChildIdAsync(childId);
            var child = administeredDosesToChild.FirstOrDefault().Child;
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
                    VaccineName = remainingDose.VaccinationSchemeDetail.Vaccine.Name,
                    CanBePut = remainingDose.CanBePut
                };
                remainingDosesToAdministerToReturn.Add(remainingDoseDTO);
            }

            return remainingDosesToAdministerToReturn;
        }

        public async Task<int> CreateAdministeredDose(AdministeredDoseCreationDTO administeredDoseCreationDTO)
        {
            var administeredDose = new AdministeredDose()
            {
                DoseDetailId = administeredDoseCreationDTO.DoseDetailId,
                ChildId = administeredDoseCreationDTO.ChildId,
                HealthCenterId = administeredDoseCreationDTO.HealthCenterId,
                HealthPersonnelId = administeredDoseCreationDTO.HealthPersonnelId,
                DoseDate = administeredDoseCreationDTO.DoseDate
            };
            return await _administeredDoseRepository.CreateAsync(administeredDose);
        }

        public async Task<List<AdministeredDoseDTO>> GetAdministeredDosesByChild(int childId)
        {
            var administeredDosesToChild = await _administeredDoseRepository.GetByChildIdWithAllRelatedInfoAsync(childId);
            var administeredDosesToChildToReturn = new List<AdministeredDoseDTO>();
            foreach (var administeredDose in administeredDosesToChild)
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
                };

                administeredDosesToChildToReturn.Add(administeredDoseDTO);
            }
            return administeredDosesToChildToReturn;
        }
    }
}
