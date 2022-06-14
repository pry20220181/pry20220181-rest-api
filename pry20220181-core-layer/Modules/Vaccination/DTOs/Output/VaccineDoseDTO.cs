using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Output
{
    public class VaccineDoseDTO
    {
        public int VaccineDoseId { get; set; }
        public int DoseNumber { get; set; }
        public string PutWhen { get; set; }
    }
}
