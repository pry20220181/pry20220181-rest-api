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

        public ChildService(IChildRepository childRepository, IVaccinationSchemeDetailRepository vaccinationSchemeDetailRepository, IAdministeredDoseRepository administeredDoseRepository)
        {
            _childRepository = childRepository;
            _vaccinationSchemeDetailRepository = vaccinationSchemeDetailRepository;
            _administeredDoseRepository = administeredDoseRepository;
        }

        public async Task<ChildDTO> GetChildByDniAsync(string DNI)
        {
            var childFromDb = await _childRepository.GetByDniAsync(DNI);
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
            var allVaccinationSchemesFromDb = await _vaccinationSchemeDetailRepository.GetAllWithVaccinesAndDosesAsync();
            //var administeredDosesToChildFromDb = await _administeredDoseRepository.GetByChildIdWithAllRelatedInfoAsync(childId);//TODO: Analizar si basta con solo traer los datos de sus tablas, sin data relacionada
            var administeredDosesToChildFromDb = await _administeredDoseRepository.GetByChildIdAsync(childId);

            var vaccinationSchemesToReturn = new List<VaccinationCardDTO.VaccinationScheme>();

            //As I iterate trough the vaccinationSchemeDetails and a VaccinationScheme has several details
            //I add the vaccinationScheme id in this list to avoid duplicates, in the Loop with get all the vaccinationSchemeDetails of this vaccinationScheme
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
                            
                            if (administeredDosesToChildFromDb.Exists(ad=>ad.DoseDetailId == vaccineDose.DoseDetailId))
                            {
                                var administeredDose = administeredDosesToChildFromDb
                                    .FirstOrDefault(ad => ad.DoseDetailId == vaccineDose.DoseDetailId);
                                vaccineDoseToReturn.Administered = true;
                                vaccineDoseToReturn.AdministeredDoseId = administeredDose.AdministeredDoseId;
                                vaccineDoseToReturn.AdministrationDate = administeredDose.DoseDate;
                                vaccineToReturn.NumberOfDosesAdministered++;
                            }
                            
                            vaccineToReturn.Doses.Add(vaccineDoseToReturn); 
                        }
                        vaccinationSchemeToReturn.Vaccines.Add(vaccineToReturn);
                    }

                    vaccinationSchemeToReturn.Complete = vaccinationSchemeToReturn.Vaccines.All(v => v.Complete);

                    vaccinationSchemesToReturn.Add(vaccinationSchemeToReturn);
                    alreadyRegisteredVaccinationSchemes.Add(vaccinationScheme.VaccinationSchemeId);
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

                //return new VaccinationCardDTO()
                //{
                //    Child = new ChildDTO()
                //    {
                //        ChildId = childFromDb.ChildId,
                //        DNI = childFromDb.DNI,
                //        Firstname = childFromDb.Firstname,
                //        Lastname = childFromDb.Lastname,
                //        Birthdate = childFromDb.Birthdate,
                //        Gender = childFromDb.Gender,
                //        Age = GetAgeFromBirthdate.GetAge(childFromDb.Birthdate)
                //    },
                //    VaccinationSchemes = new List<VaccinationCardDTO.VaccinationScheme>()
                //    {
                //        new VaccinationCardDTO.VaccinationScheme()
                //        {
                //            Name = "Vacunacion niños menor 1 año",
                //            Vaccines = new List<VaccinationCardDTO.VaccinationScheme.Vaccine>()
                //            {
                //               new VaccinationCardDTO.VaccinationScheme.Vaccine()
                //               {
                //                   Name = "Pentavalente",
                //                   Doses = new List<VaccinationCardDTO.VaccinationScheme.Vaccine.Dose>()
                //                   {
                //                       new VaccinationCardDTO.VaccinationScheme.Vaccine.Dose()
                //                       {
                //                           DoseNumber = 1
                //                       },
                //                       new VaccinationCardDTO.VaccinationScheme.Vaccine.Dose()
                //                       {
                //                           DoseNumber = 2
                //                       }
                //                   }
                //               }
                //            }
                //        }
                //    }
                //};
            }
    }
}
