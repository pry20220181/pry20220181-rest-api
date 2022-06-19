using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Output
{
    public class RemainingDoseDTO
    {
        public int RemainingDoseId { get; set; }
        public string VaccineName { get; set; }
        public int DoseNumber { get; set; }
        public string VaccinationSchemeName { get; set; }
        public string PutWhen { get; set; }
        public string PossibleEffectsPostVaccine { get; set; }
        public int VaccinationSchemeInitialAge { get; set; }
        public int VaccinationSchemeFinalAge { get; set; }
        public int VaccinationSchemeDetailId { get; set; }
        public int VaccinationSchemeId { get; set; }
        public bool CanBePut { get; set; }
    }
}
