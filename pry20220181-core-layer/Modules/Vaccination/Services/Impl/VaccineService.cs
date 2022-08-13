using Microsoft.Extensions.Logging;
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
using static pry20220181_core_layer.Modules.Master.DTOs.Output.VaccinationCardDTO.VaccinationScheme.Vaccine;

namespace pry20220181_core_layer.Modules.Vaccination.Services.Impl
{
    public class VaccineService : IVaccineService
    {
        IVaccineRepository _vaccineRepository { get; set; }
        IVaccinationSchemeRepository _vaccinationSchemeRepository { get; set; }
        IVaccinationSchemeDetailRepository _vaccinationSchemeDetailRepository { get; set; }
        IDoseDetailRepository _doseDetailRepository { get; set; }
        ILogger<VaccineService> _logger { get; set; }

        public VaccineService(IVaccineRepository vaccineRepository, IVaccinationSchemeRepository vaccinationSchemeRepository, IVaccinationSchemeDetailRepository vaccinationSchemeDetailRepository, IDoseDetailRepository doseDetailRepository, ILogger<VaccineService> logger)
        {
            _vaccineRepository = vaccineRepository;
            _vaccinationSchemeRepository = vaccinationSchemeRepository;
            _vaccinationSchemeDetailRepository = vaccinationSchemeDetailRepository;
            _doseDetailRepository = doseDetailRepository;
            _logger = logger;
        }

        public async Task<VaccineDTO> GetVaccineByIdAsync(int id)
        {
            //TODO: Implement Validation logic
            var vaccineFromDb = await _vaccineRepository.GetByIdAsync(id);

            var vaccineToReturn = new VaccineDTO()
            {
                Id = vaccineFromDb.VaccineId,
                Name = vaccineFromDb.Name,
                Description = vaccineFromDb.Description,
                MinTemperature = vaccineFromDb.MinTemperature,
                MaxTemperature = vaccineFromDb.MaxTemperature,
            };

            return vaccineToReturn;
        }

        public async Task<int> CreateVaccineAsync(VaccineCreationDTO vaccineCreationDTO)
        {
            #region Register Vaccine
            var vaccineToCreate = new Vaccine()
            {
                Name = vaccineCreationDTO.Name,
                Description = vaccineCreationDTO.Description,
                MinTemperature = vaccineCreationDTO.MinTemperature,
                MaxTemperature = vaccineCreationDTO.MaxTemperature,
            };

            var vaccineId = await _vaccineRepository.CreateAsync(vaccineToCreate);

            if(vaccineId > 0)
            {
                _logger.LogInformation($"The Vaccine with ID {vaccineId} was created");
            }
            else
            {
                _logger.LogInformation($"Something was wrong creating the vaccine {vaccineCreationDTO.Name}");
                return 0;
            }

            #endregion

            var vaccinationSchemes = new List<VaccinationScheme>();
            var vaccinatioSchemeDetailToCreate = new List<VaccinationSchemeDetail>();
            var vaccineDosesToCreate = new List<DoseDetail>();

            #region Assign the new Vaccine to its VaccinationScheme (The Table VaccinationSchemeDetail is the M:M table of the relationship between Vaccine and VaccinationScheme)
            //One Vaccine can have many VaccinationSchemes and one VaccinationScheme can have many Vaccines.
            //As this table is the Intermediate table between Vaccine and VaccinationScheme, we create all the VaccinationSchemeDetail
            //That arrives, because it indicate that "this vaccine is available in this VaccinationScheme".
            foreach (var vaccinationSchemeItem in vaccineCreationDTO.VaccinationSchemes)
            {
                VaccinationSchemeDetail vaccinationSchemeDetailToCreate = new VaccinationSchemeDetail()
                {
                    NumberOfDosesToAdminister = vaccinationSchemeItem.NumberOfDoses,
                    PossibleEffectsPostVaccine = vaccinationSchemeItem.PossibleEffectsPostVaccine,
                    VaccineId = vaccineId,
                    VaccinationSchemeId = vaccinationSchemeItem.VaccinationSchemeId
                };
                vaccinatioSchemeDetailToCreate.Add(vaccinationSchemeDetailToCreate);
            }
            var createdVaccinationSchemeDetails = await _vaccinationSchemeDetailRepository.CreateRangeAsync(vaccinatioSchemeDetailToCreate);
            _logger.LogInformation($"{createdVaccinationSchemeDetails.Count()} vaccination scheme details created for the Vaccine {vaccineCreationDTO.Name}");
            #endregion

            #region We register all the Details (Doses) about the vaccinationSchemeDetail of this Vaccine
            foreach (var vaccinationSchemeItem in vaccineCreationDTO.VaccinationSchemes)
            {
                var vaccinationSchemeDetailId = createdVaccinationSchemeDetails
                    .FirstOrDefault(v => v.VaccinationSchemeId == vaccinationSchemeItem.VaccinationSchemeId)
                    .VaccinationSchemeDetailId;
                foreach (var vaccineDose in vaccinationSchemeItem.VaccineDoses)
                {
                    DoseDetail doseDetailToCreate = new DoseDetail()
                    {
                        DoseNumber = vaccineDose.DoseNumber,
                        // PutWhenNewBorn = vaccineDose.PutWhenNewBorn,
                        PutWhenHasMonths = vaccineDose.PutWhenHasMonths,
                        // PutMonthsAfterPreviousDosis = vaccineDose.PutMonthsAfterPreviousDosis,
                        // PutBetweenStartMonth = vaccineDose.PutBetweenStartMonth,
                        // PutBetweenEndMonth = vaccineDose.PutBetweenEndMonth,
                        // PutEveryYear = vaccineDose.PutEveryYear,
                        VaccinationSchemeDetailId = vaccinationSchemeDetailId
                    };
                    vaccineDosesToCreate.Add(doseDetailToCreate);
                }
            }

            var createdDoseDetails = await _doseDetailRepository.CreateRangeAsync(vaccineDosesToCreate);
            _logger.LogInformation($"{createdDoseDetails.Count()} doses created for the Vaccine {vaccineCreationDTO.Name}");
            #endregion


            return vaccineId;
        }

        public async Task<VaccineDTO> UpdateVaccineAsync(int id, VaccineUpdateDTO vaccineUpdateDTO)
        {
            //TODO: Implement Validation logic
            var vaccineToUpdate = new Vaccine()
            {
                VaccineId = id,
                Name = vaccineUpdateDTO.Name,
                Description = vaccineUpdateDTO.Description,
                MinTemperature = vaccineUpdateDTO.MinTemperature,
                MaxTemperature = vaccineUpdateDTO.MaxTemperature
            };

            var updatedVaccine = await _vaccineRepository.UpdateAsync(vaccineToUpdate);

            var vaccineToReturn = new VaccineDTO()
            {
                Id = updatedVaccine.VaccineId,
                Name = updatedVaccine.Name,
                Description = updatedVaccine.Description,
                MinTemperature = updatedVaccine.MinTemperature,
                MaxTemperature = updatedVaccine.MaxTemperature
            };

            return vaccineToReturn;
        }

        public async Task<bool> DeleteVaccineAsync(int id)
        {
            var result = await _vaccineRepository.DeleteAsync(id);

            return result;
        }

        public async Task<List<VaccineDTO>> GetVaccinesCompleteInfoAsync(PaginationParameter paginationParameter, string fields)
        {
            var vaccinesToReturn = new List<VaccineDTO>();
            var vaccinesFromDb = (fields == GetVaccinesMode.WithAllInfo ? await _vaccineRepository.GetWithSchemesAndDosesAsync(paginationParameter) : await _vaccineRepository.GetMinimalInfoAsync(paginationParameter));
            _logger.LogTrace($"The vaccine repository returned {vaccinesFromDb.Count} vaccines");
            foreach (var vaccine in vaccinesFromDb)
            {
                var vaccineToReturn = new VaccineDTO()
                {
                    Id = vaccine.VaccineId,
                    Name = vaccine.Name
                };

                if (fields == GetVaccinesMode.WithAllInfo)
                {
                    vaccineToReturn.Description = vaccine.Description;
                    vaccineToReturn.MinTemperature = vaccine.MinTemperature;
                    vaccineToReturn.MaxTemperature = vaccine.MaxTemperature;

                    foreach (var vaccinationScheme in vaccine.VaccinationSchemeDetails)
                    {
                        var vaccinationSchemeToReturn = new VaccinationSchemeDTO()
                        {
                            VaccinationSchemeId = vaccinationScheme.VaccinationSchemeId,
                            Name = vaccinationScheme.VaccinationScheme.Name,
                            InitialAge = vaccinationScheme.VaccinationScheme.InitialAge,
                            FinalAge = vaccinationScheme.VaccinationScheme.FinalAge,
                            NumberOfDoses = vaccinationScheme.NumberOfDosesToAdminister,
                            PossibleEffectsPostVaccine = vaccinationScheme.PossibleEffectsPostVaccine
                        };

                        foreach (var dose in vaccinationScheme.DosesDetails)
                        {
                            var vaccineDose = new VaccineDoseDTO()
                            {
                                VaccineDoseId = dose.DoseDetailId,
                                DoseNumber = dose.DoseNumber,
                                PutWhen = WhenPutVaccine.ToString(dose),
                            };

                            vaccinationSchemeToReturn.VaccineDoses.Add(vaccineDose);
                        }

                        vaccineToReturn.VaccinationSchemes.Add(vaccinationSchemeToReturn);
                    };
                }

                vaccinesToReturn.Add(vaccineToReturn);
            }

            return vaccinesToReturn;
        }

        public async Task<VaccineDTO> GetVaccineCompleteInfoByIdAsync(int id)
        {
            if(id < 1)
            {
                return null;
            }

            var vaccineFromDb = await _vaccineRepository.GetByIdWithSchemesAndDosesAsync(id);

            if(vaccineFromDb is null)
            {
                _logger.LogInformation($"The Vaccine with ID {id} does not exist");
                return null;
            }

            var vaccineToReturn = new VaccineDTO()
            {
                Id = vaccineFromDb.VaccineId,
                Name = vaccineFromDb.Name,
                Description = vaccineFromDb.Description,
                MinTemperature = vaccineFromDb.MinTemperature,
                MaxTemperature = vaccineFromDb.MaxTemperature
            };

            _logger.LogInformation($"The Vaccine with ID {id} has {vaccineFromDb.VaccinationSchemeDetails.Count()} Schemes");
            foreach (var vaccinationScheme in vaccineFromDb.VaccinationSchemeDetails)
            {
                var vaccinationSchemeToReturn = new VaccinationSchemeDTO()
                {
                    VaccinationSchemeId = vaccinationScheme.VaccinationSchemeId,
                    Name = vaccinationScheme.VaccinationScheme.Name,
                    InitialAge = vaccinationScheme.VaccinationScheme.InitialAge,
                    FinalAge = vaccinationScheme.VaccinationScheme.FinalAge,
                    NumberOfDoses = vaccinationScheme.NumberOfDosesToAdminister,
                    PossibleEffectsPostVaccine = vaccinationScheme.PossibleEffectsPostVaccine
                };

                _logger.LogInformation($"The Vaccine with ID {id} in its Scheme {vaccinationScheme.VaccinationScheme.Name} has {vaccinationScheme.DosesDetails.Count()} doses");
                vaccinationSchemeToReturn.VaccineDoses = vaccinationScheme.DosesDetails.Select(dose => new VaccineDoseDTO()
                {
                    VaccineDoseId = dose.DoseDetailId,
                    DoseNumber = dose.DoseNumber,
                    PutWhen = WhenPutVaccine.ToString(dose),
                }).ToList();

                vaccineToReturn.VaccinationSchemes.Add(vaccinationSchemeToReturn);
            };

            return vaccineToReturn;
        }
    }
}
