using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
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
            var remainingDosesToAdminister = allDosesFromDb
                .Where(d => administeredDosesToChild
                .Exists(ad => ad.DoseDetailId == d.DoseDetailId) == false)
                .ToList();

            var remainingDosesToAdministerToReturn = new List<RemainingDoseDTO>();
            foreach (var remainingDose in remainingDosesToAdminister)
            {
                RemainingDoseDTO remainingDoseDTO = new RemainingDoseDTO()
                {
                    RemainingDoseId = remainingDose.DoseDetailId,
                    VaccinationSchemeDetailId = remainingDose.VaccinationSchemeDetailId,
                    DoseNumber = remainingDose.DoseNumber,
                    PutWhen = WhenPutVaccine.ToString(remainingDose),
                    //TODO: Agregar mas detalle del esquema y de la vacuna
                };
                remainingDosesToAdministerToReturn.Add(remainingDoseDTO);
            }
            return remainingDosesToAdministerToReturn;
        }
    }
}
