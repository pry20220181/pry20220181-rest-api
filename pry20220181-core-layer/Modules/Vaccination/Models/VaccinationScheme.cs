using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class VaccinationScheme
    {
        public int VaccinationSchemeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InitialAge { get; set; }
        public int FinalAge { get; set; }
        
        #region Relations with another tables
        public List<VaccinationSchemeDetail> VaccinationSchemeDetails { get; set; }
        #endregion
    }
}
