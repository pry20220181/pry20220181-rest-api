using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
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

        public DosesService(IDoseDetailRepository dosesRepository)
        {
            _doseDetailRepository = dosesRepository;
        }

        public Task<List<RemainingDoseDTO>> GetRemainingDosesByChild(int childId)
        {
            var allDosesFromDb = _doseDetailRepository.GetAllWithSchemesAndVaccinesAsync();
            return null;
        }
    }
}
