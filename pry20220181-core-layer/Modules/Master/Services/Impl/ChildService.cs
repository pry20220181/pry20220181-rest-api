using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class ChildService : IChildService
    {
        private readonly IChildRepository _childRepository;
        private readonly IVaccinationSchemeDetailRepository _vaccinationSchemeDetailRepository;
        private readonly IAdministeredDoseRepository _administeredDoseRepository;
        private ILogger<ChildService> _logger { get; set; }
        public ChildService(IChildRepository childRepository, IVaccinationSchemeDetailRepository vaccinationSchemeDetailRepository, IAdministeredDoseRepository administeredDoseRepository, ILogger<ChildService> logger)
        {
            _childRepository = childRepository;
            _vaccinationSchemeDetailRepository = vaccinationSchemeDetailRepository;
            _administeredDoseRepository = administeredDoseRepository;
            _logger = logger;
        }

        public async Task<ChildDTO> GetChildByDniAsync(string DNI)
        {
            var childFromDb = await _childRepository.GetByDniAsync(DNI);

            if(childFromDb is null)
            {
                return null;
            }

            var childToReturn = new ChildDTO()
            {
                ChildId = childFromDb.ChildId,
                DNI = childFromDb.DNI,
                Firstname = childFromDb.Firstname,
                Lastname = childFromDb.Lastname,
                Birthdate = childFromDb.Birthdate,
                Gender = childFromDb.Gender,
                Age = GetAgeFromBirthdate.GetAge(childFromDb.Birthdate)
            };
            return childToReturn;
        }

        public async Task<VaccinationCardDTO> GetVaccinationCardAsync(int childId)
        {
            var childFromDb = await _childRepository.GetByIdAsync(childId);

            if (childFromDb is null)
            {
                _logger.LogError($"The Child with ID {childId} does not exist");
                return null;
            }

            var allVaccinationSchemesFromDb = await _vaccinationSchemeDetailRepository.GetAllWithVaccinesAndDosesAsync();
            _logger.LogInformation($"Got {allVaccinationSchemesFromDb.Count} Vaccination Schemes from DB (with its related info: Vaccines and Doses)");

            var administeredDosesToChildFromDb = await _administeredDoseRepository.GetByChildIdWithAllRelatedInfoAsync(childId);
            _logger.LogInformation($"The Child with ID {childId} has {administeredDosesToChildFromDb} administered doses");

            var vaccinationSchemesToReturn = new List<VaccinationCardDTO.VaccinationScheme>();

            /// <description>
            /// As I iterate trough the vaccinationSchemeDetails and a VaccinationScheme has several details
            /// I add the vaccinationScheme Id in this list to avoid duplicates, in the Loop with get all the vaccinationSchemeDetails of this vaccinationScheme
            /// </description>
            List<int> alreadyRegisteredVaccinationSchemes = new List<int>();

            foreach (var vaccinationScheme in allVaccinationSchemesFromDb)
            {
                if (!alreadyRegisteredVaccinationSchemes.Contains(vaccinationScheme.VaccinationSchemeId))
                {
                    var vaccinationSchemeToReturn = new VaccinationCardDTO.VaccinationScheme()
                    {
                        VaccinationSchemeId = vaccinationScheme.VaccinationSchemeId,
                        Name = vaccinationScheme.VaccinationScheme.Name,
                        InitialAge = vaccinationScheme.VaccinationScheme.InitialAge,
                        FinalAge = vaccinationScheme.VaccinationScheme.FinalAge,
                    };

                    //These are the details of the present vaccination scheme, each detail has the vaccines of this scheme
                    var vaccinationSchemeDetails = allVaccinationSchemesFromDb
                        .Where(v => v.VaccinationSchemeId == vaccinationScheme.VaccinationSchemeId).ToList();

                    foreach (var vaccinationSchemeDetail in vaccinationSchemeDetails)
                    {
                        var vaccineToReturn = new VaccinationCardDTO.VaccinationScheme.Vaccine()
                        {
                            VaccineId = vaccinationSchemeDetail.Vaccine.VaccineId,
                            Name = vaccinationSchemeDetail.Vaccine.Name,
                            NumberOfDosesToAdminister = vaccinationSchemeDetail.NumberOfDosesToAdminister,
                            NumberOfDosesAdministered = 0
                        };

                        var vaccineDosesToReturn = vaccinationSchemeDetail.DosesDetails
                            .Where(d => d.VaccinationSchemeDetailId == vaccinationSchemeDetail.VaccinationSchemeDetailId)
                            .ToList();

                        foreach (var vaccineDose in vaccineDosesToReturn)
                        {
                            var vaccineDoseToReturn = new VaccinationCardDTO.VaccinationScheme.Vaccine.Dose()
                            {
                                DoseId = vaccineDose.DoseDetailId,
                                DoseNumber = vaccineDose.DoseNumber,
                                Administered = false,
                                PutWhen = WhenPutVaccine.ToString(vaccineDose)
                            };

                            var administeredDose = administeredDosesToChildFromDb
                                    .FirstOrDefault(ad => ad.DoseDetailId == vaccineDose.DoseDetailId);
                            if (!(administeredDose is null))
                            {
                                vaccineDoseToReturn.Administered = true;
                                vaccineDoseToReturn.AdministeredDoseId = administeredDose.AdministeredDoseId;
                                vaccineDoseToReturn.AdministrationDate = administeredDose.DoseDate;
                                vaccineDoseToReturn.HealthPersonnel = new VaccinationCardDTO.VaccinationScheme.Vaccine.HealthPersonnel(){
                                    HealthPersonnelId = administeredDose.HealthPersonnelId,
                                    Fullname = administeredDose.HealthPersonnel.User.FirstName + " " + administeredDose.HealthPersonnel.User.LastName
                                };
                                vaccineDoseToReturn.Observations = administeredDose.Observations;
                                vaccineToReturn.NumberOfDosesAdministered++;
                            }

                            vaccineToReturn.Doses.Add(vaccineDoseToReturn);
                        }
                        vaccinationSchemeToReturn.Vaccines.Add(vaccineToReturn);
                    }

                    vaccinationSchemeToReturn.Complete = vaccinationSchemeToReturn.Vaccines.All(v => v.Complete);

                    vaccinationSchemesToReturn.Add(vaccinationSchemeToReturn);
                    alreadyRegisteredVaccinationSchemes.Add(vaccinationScheme.VaccinationSchemeId);
                    _logger.LogInformation($"The Vaccination Scheme with ID {vaccinationScheme.VaccinationSchemeId} was added to the List to return with its related info");
                }
            }

            return new VaccinationCardDTO()
            {
                Child = new ChildDTO()
                {
                    ChildId = childFromDb.ChildId,
                    DNI = childFromDb.DNI,
                    Firstname = childFromDb.Firstname,
                    Lastname = childFromDb.Lastname,
                    Birthdate = childFromDb.Birthdate,
                    Gender = childFromDb.Gender,
                    Age = GetAgeFromBirthdate.GetAge(childFromDb.Birthdate)
                },
                VaccinationSchemes = vaccinationSchemesToReturn
            };
        }
    }
}
