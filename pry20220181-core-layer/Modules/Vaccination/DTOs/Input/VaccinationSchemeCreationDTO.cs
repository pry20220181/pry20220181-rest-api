using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Input
{
    public class VaccinationSchemeCreationDTO
    {
        public int VaccinationSchemeId { get; set; }
        public int NumberOfDoses { get; set; }
        public string PossibleEffectsPostVaccine { get; set; }
        // public List<VaccineDoseCreationDTO> VaccineDoses { get; set; } = new List<VaccineDoseCreationDTO>();
    }
}
