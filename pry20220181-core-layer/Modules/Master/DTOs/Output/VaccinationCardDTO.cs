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
            public int VaccinationSchemeId { get; set; }
            public string Name { get; set; }
            public int InitialAge { get; set; }
            public int FinalAge { get; set; }
            /// <summary>
            /// Indicates if all doses of vaccines of this Scheme have been administered
            /// </summary>
            public bool Complete { get; set; }
            public List<Vaccine> Vaccines { get; set; } = new List<Vaccine>();

            public class Vaccine
            {
                public int VaccineId { get; set; }
                public string Name { get; set; }
                public int NumberOfDosesToAdminister { get; set; }
                public int NumberOfDosesAdministered { get; set; }
                public bool Complete { get { return NumberOfDosesToAdminister == NumberOfDosesAdministered; } }
                public List<Dose> Doses { get; set; } = new List<Dose>();


                public class Dose
                {
                    public int DoseId { get; set; }
                    public int DoseNumber { get; set; }
                    public bool Administered { get; set; }
                    public int AdministeredDoseId { get; set; }
                    public DateTime? AdministrationDate { get; set; }
                    public string PutWhen { get; set; }
                }
            }
        }
    }

}
