using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class Vaccine
    {
        public int VaccineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }

        #region Relation with other tables
        public List<VaccinationSchemeDetail> VaccinationSchemeDetails { get; set; }
        #endregion
    }
}
