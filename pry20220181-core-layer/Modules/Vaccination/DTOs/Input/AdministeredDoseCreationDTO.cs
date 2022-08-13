using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Input
{
    public class AdministeredDoseCreationDTO
    {
        public int DoseDetailId { get; set; }
        public int ChildId { get; set; }
        public int HealthCenterId { get; set; }
        public int HealthPersonnelId { get; set; }
        public DateTime DoseDate { get; set; } = DateTime.UtcNow.AddHours(-5);
        public string Observations { get; set; }
    }
}
