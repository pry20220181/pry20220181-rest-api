using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services.Impl
{
    public class VaccinationSchemeService : IVaccinationSchemeService
    {
        private readonly IVaccinationSchemeRepository _vaccinationSchemeRepository;

        public VaccinationSchemeService(IVaccinationSchemeRepository vaccinationSchemeRepository)
        {
            _vaccinationSchemeRepository = vaccinationSchemeRepository;
        }

        public async Task<List<VaccinationSchemeDTO>> GetAllVaccinationSchemes()
        {
            var schemesFromDb = await _vaccinationSchemeRepository.GetAllAsync();

            return schemesFromDb.Select(s => new VaccinationSchemeDTO()
            {
                VaccinationSchemeId = s.VaccinationSchemeId,
                Name = s.Name,
                Description = s.Name,
                InitialAge = s.InitialAge,
                FinalAge = s.FinalAge
            }).ToList();
        }
    }
}
