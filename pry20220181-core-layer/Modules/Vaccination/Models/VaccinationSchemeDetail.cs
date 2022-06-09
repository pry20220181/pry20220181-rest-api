using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class VaccinationSchemeDetail
    {
        public int VaccinationSchemeDetailId { get; set; }
        public int VaccineId { get; set; }
        public int VaccinationSchemeId { get; set; }
        public int NumberOfDosesToAdminister { get; set; }
        public string PossibleEffectsPostVaccine { get; set; }

        #region Relations with another tables
        public Vaccine Vaccine { get; set; }
        public VaccinationScheme VaccinationScheme { get; set; }
        #endregion
    }
}
