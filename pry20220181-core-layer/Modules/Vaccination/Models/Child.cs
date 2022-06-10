using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class Child
    {
        public int ChildId { get; set; }
        public string DNI { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public char Gender { get; set; }

        #region Relations with another tables
        public List<ChildParent> ChildParents { get; set; }
        public List<AdministeredDose> AdministeredDoses { get; set; }
        #endregion
    }
}
