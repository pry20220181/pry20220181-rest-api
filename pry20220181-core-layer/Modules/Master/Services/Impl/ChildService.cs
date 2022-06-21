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
            var administeredDosesToChildFromDb = await _administeredDoseRepository.GetByChildIdWithAllRelatedInfoAsync(childId);

            var vaccinationSchemesToReturn = new List<VaccinationCardDTO.VaccinationScheme>();

            List<int> alreadyRegisteredVaccinationSchemes = new List<int>();
            foreach (var vaccinationScheme in allVaccinationSchemesFromDb)
            {
                if (!alreadyRegisteredVaccinationSchemes.Contains(vaccinationScheme.VaccinationSchemeId))
                {
                    var vaccinationSchemeToReturn = new VaccinationCardDTO.VaccinationScheme()
                    {
                        Name = vaccinationScheme.VaccinationScheme.Name
                    };
                    //These are the details of the present vaccination scheme, each detail has the vaccines of this scheme
                    var vaccinationSchemeDetails = allVaccinationSchemesFromDb
                        .Where(v => v.VaccinationSchemeId == vaccinationScheme.VaccinationSchemeId).ToList();
                    foreach (var vaccinationSchemeDetail in vaccinationSchemeDetails)
                    {
                        vaccinationSchemeToReturn.Vaccines.Add(new VaccinationCardDTO.VaccinationScheme.Vaccine()
                        {
                            Name = vaccinationSchemeDetail.Vaccine.Name,
                            NumberOfDosesToAdminister = vaccinationSchemeDetail.NumberOfDosesToAdminister
                        });
                    }

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
