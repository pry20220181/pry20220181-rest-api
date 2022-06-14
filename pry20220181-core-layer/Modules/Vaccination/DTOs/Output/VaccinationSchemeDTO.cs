using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Output
{
    public class VaccinationSchemeDTO
    {
        public int VaccinationSchemeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InitialAge { get; set; }
        public int FinalAge { get; set; }
        public int NumberOfDoses { get; set; }
        public string PossibleEffectsPostVaccine { get; set; }
        public List<VaccineDoseDTO> VaccineDoses { get; set; } = new List<VaccineDoseDTO>();
    }
}
