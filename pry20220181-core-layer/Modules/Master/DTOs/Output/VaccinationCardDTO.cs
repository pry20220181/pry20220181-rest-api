using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class VaccinationCardDTO
    {
        public ChildDTO Child { get; set; }
        public List<VaccinationScheme> VaccinationSchemes { get; set; } = new List<VaccinationScheme>();


        public class VaccinationScheme
        {
            public string Name { get; set; }
            public List<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
            
            
            public class Vaccine
            {
                public string Name { get; set; }
                public int NumberOfDosesToAdminister { get; set; }
                public List<Dose> Doses { get; set; } = new List<Dose>();

                
                public class Dose
                {
                    public int DoseNumber { get; set; }
                }
            }
        }
    }
    
}
