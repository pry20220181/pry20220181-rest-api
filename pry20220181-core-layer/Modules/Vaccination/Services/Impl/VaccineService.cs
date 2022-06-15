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
    public class VaccineService : IVaccineService
    {
        IVaccineRepository _vaccineRepository { get; set; }
        IVaccinationSchemeRepository _vaccinationSchemeRepository { get; set; }
        IVaccinationSchemeDetailRepository _vaccinationSchemeDetailRepository { get; set; }
        IDoseDetailRepository _doseDetailRepository { get; set; }

        public VaccineService(IVaccineRepository vaccineRepository, IVaccinationSchemeRepository vaccinationSchemeRepository, IVaccinationSchemeDetailRepository vaccinationSchemeDetailRepository, IDoseDetailRepository doseDetailRepository)
        {
            _vaccineRepository = vaccineRepository;
            _vaccinationSchemeRepository = vaccinationSchemeRepository;
            _vaccinationSchemeDetailRepository = vaccinationSchemeDetailRepository;
            _doseDetailRepository = doseDetailRepository;
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

        public async Task<List<VaccineDTO>> GetVaccinesAsync(PaginationParameter paginationParameter, string fields = "all")
        {
            //TODO: Implement Validation logic
            var vaccinesToReturn = new List<VaccineDTO>();
            var vaccinesFromDb = await _vaccineRepository.GetAsync(paginationParameter, fields);
            foreach (var vaccine in vaccinesFromDb)
            {
                var vaccineToReturn = new VaccineDTO()
                {
                    Id = vaccine.VaccineId,
                    Name = vaccine.Name
                };
                
                if (fields == "all")
                {
                    vaccineToReturn.Description = vaccine.Description;
                    vaccineToReturn.MinTemperature = vaccine.MinTemperature;
                    vaccineToReturn.MaxTemperature = vaccine.MaxTemperature;
                }
                
                vaccinesToReturn.Add(vaccineToReturn);
            }
            return vaccinesToReturn;
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
            #endregion

            //TODO: Obtener las listas de la DB y revisar cuales agregar a la lista INsert y tomar los ID en caso

            #region Map Objects from DTO to Model
            var vaccinationSchemesToCreate = new List<VaccinationScheme>();
            var vaccinatioSchemeDetailToCreate = new List<VaccinationSchemeDetail>();
            var vaccineDosesToCreate = new List<DoseDetail>();
            foreach (var vaccinationScheme in vaccineCreationDTO.VaccinationSchemes)
            {
                VaccinationScheme vaccinationSchemeToCreate = new VaccinationScheme()
                {
                    Name = vaccinationScheme.Name,
                    InitialAge = vaccinationScheme.InitialAge,
                    FinalAge = vaccinationScheme.FinalAge
                };
                vaccinationSchemesToCreate.Add(vaccinationSchemeToCreate);

                VaccinationSchemeDetail vaccinationSchemeDetailToCreate = new VaccinationSchemeDetail()
                {
                    NumberOfDosesToAdminister = vaccinationScheme.NumberOfDoses,
                    PossibleEffectsPostVaccine = vaccinationScheme.PossibleEffectsPostVaccine,
                    VaccineId = vaccineId,
                    VaccinationSchemeId = vaccinationSchemeToCreate.VaccinationSchemeId
                };
                vaccinatioSchemeDetailToCreate.Add(vaccinationSchemeDetailToCreate);

                foreach (var vaccineDose in vaccinationScheme.VaccineDoses)
                {
                    DoseDetail doseDetailToCreate = new DoseDetail()
                    {
                        DoseNumber = vaccineDose.DoseNumber,
                        PutWhenNewBorn = vaccineDose.PutWhenNewBorn,
                        PutWhenHasMonths = vaccineDose.PutWhenHasMonths,
                        PutMonthsAfterPreviousDosis = vaccineDose.PutMonthsAfterPreviousDosis,
                        PutBetweenStartMonth = vaccineDose.PutBetweenStartMonth,
                        PutBetweenEndMonth = vaccineDose.PutBetweenEndMonth,
                        PutEveryYear = vaccineDose.PutEveryYear,
                        VaccinationSchemeDetailId = vaccinationSchemeDetailToCreate.VaccinationSchemeDetailId
                    };
                    vaccineDosesToCreate.Add(doseDetailToCreate);
                }
            }
            #endregion

            #region Register VaccinationScheme, VaccinationSchemeDetail and DoseDetail
            //TODO: Revisar el mapeo de IDs, porque se asignan cuando se insertan, pero no se mapean en sus relaciones, una opci[on es recorrerlos y asignar,
            //pero no hay forma e relacionarlos creo, el 15/6/22 se vera con mas detalle
            var createdVaccineSchemes = await _vaccinationSchemeRepository.CreateRangeAsync(vaccinationSchemesToCreate);
            var createdVaccinationSchemeDetails = await _vaccinationSchemeDetailRepository.CreateRangeAsync(vaccinatioSchemeDetailToCreate);
            var createdDoseDetails = await _doseDetailRepository.CreateRangeAsync(vaccineDosesToCreate);
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
            var vaccinesFromDb = await _vaccineRepository.GetWithSchemesAndDosesAsync(paginationParameter, fields);
            foreach (var vaccine in vaccinesFromDb)
            {
                var vaccineToReturn = new VaccineDTO()
                {
                    Id = vaccine.VaccineId,
                    Name = vaccine.Name
                };

                if (fields == "all")
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
    }
}
