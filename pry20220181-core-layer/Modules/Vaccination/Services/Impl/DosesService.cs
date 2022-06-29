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

        public DosesService(IDoseDetailRepository dosesRepository, IAdministeredDoseRepository administeredDoseRepository, IChildRepository childRepository)
        {
            _doseDetailRepository = dosesRepository;
            _administeredDoseRepository = administeredDoseRepository;
            _childRepository = childRepository;
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
            var adminesteredDoseId = await _administeredDoseRepository.CreateAsync(administeredDose);
            #region Get the remaining doses of the current Scheme that are able to put
            var currentVaccinationScheme = await _doseDetailRepository.GetVaccinationSchemeByDoseDetailIdAsync(administeredDose.DoseDetailId);
            var currentVaccinationSchemeDetails = currentVaccinationScheme.VaccinationSchemeDetails;
            var currentDosesDetails = new List<DoseDetail>();
            var dosesDetailId = new List<int>();

            foreach (var vaccinationSchemeDetail in currentVaccinationSchemeDetails)
            {
                currentDosesDetails.AddRange(vaccinationSchemeDetail.DosesDetails);
                dosesDetailId.AddRange(vaccinationSchemeDetail.DosesDetails.Select(d => d.DoseDetailId));
            }

            var administeredDosesOfCurrentVaccinationScheme = await _administeredDoseRepository.GetByDosesIdList(dosesDetailId);

            var child = await _childRepository.GetByIdAsync(administeredDose.ChildId);
            var remainingDosesToAdminister = currentDosesDetails
                .Where(d => administeredDosesOfCurrentVaccinationScheme
                .Exists(ad => ad.DoseDetailId == d.DoseDetailId) == false)
                .ToList();

            remainingDosesToAdminister = DosesAnalyzer.EvaluateIfTheDosesCanBePut(child, administeredDosesOfCurrentVaccinationScheme, remainingDosesToAdminister);
            var remainingDosesToAdministerThatCanBePut = remainingDosesToAdminister.Where(d => d.CanBePut).ToList();
            #endregion
            //O SEA TENGO QUE CREAR UN RECORDATORIO PARA TODAS ESTAS DOSIS RESTANTES QUE YA SE PUEDEN PONER,
            //LO QUE FALTA ES EL CUANDO ENVIO ESTE RECORDATORIO: 28/6/22 22:26 Para no loquearme mucho, los pongo para dentro de una semana

            //AHORA PARA UN ESCENARIO MAS REAL HABRIA UN JOB QUE REVISE DE TODOS LOS NIÑOS (por departamento x ejemplo)
            //QUE VACUNAS RESTANTES ESTAN APTAS PARA QUE SE LES PONGA, Y DE ESAS CREAR LOS REMINDERS
            //TODO: Agregar revision de reminders para esta dosis, es decir revisar si hy un recordatorio por enviar sobre esta dosis (de este niño) que se acaba de registrar para eliminarlo

            return adminesteredDoseId;
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
