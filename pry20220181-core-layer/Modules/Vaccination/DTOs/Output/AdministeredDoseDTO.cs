using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Output
{
    public class AdministeredDoseDTO
    {
        public string AdministeredDoseId { get; set; }
        public string VaccineName { get; set; }
        public int DoseNumber { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string HealthCenterName { get; set; }
        public string HealthPersonnelName { get; set; }
        public string VaccinationSchemeName { get; set; }
        public string WhenShouldBePut { get; set; }
        public int VaccineId { get; set; }
        public int VaccinationSchemeDetailId { get; set; }
        public int VaccinationSchemeId { get; set; }
    }
}
