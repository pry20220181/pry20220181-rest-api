using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services
{
    public interface IDosesService
    {
        public Task<List<RemainingDoseDTO>> GetRemainingDosesByChild(int childId);
        /// <summary>
        /// Get the administered doses of the specified child
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        public Task<List<AdministeredDoseDTO>> GetAdministeredDosesByChild(int childId);
        public Task<AdministeredDoseDTO> GetAdministeredDoseByIdAsync(string administeredDoseId);
        public Task<string> CreateAdministeredDose(AdministeredDoseCreationDTO administeredDoseCreationDTO);
    }
}
