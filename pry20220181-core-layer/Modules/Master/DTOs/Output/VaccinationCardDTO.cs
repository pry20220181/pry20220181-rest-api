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
        public List<VaccinationScheme> VaccinationSchemes { get; set; }

        
        public class VaccinationScheme
        {
            public string Name { get; set; }
            public List<Vaccine> Vaccines { get; set; }
            
            
            public class Vaccine
            {
                public string Name { get; set; }
                public List<Dose> Doses { get; set; } 

                
                public class Dose
                {
                    public int DoseNumber { get; set; }
                }
            }
        }
    }
    
}
