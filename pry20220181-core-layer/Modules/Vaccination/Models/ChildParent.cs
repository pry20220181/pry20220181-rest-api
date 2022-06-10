using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class ChildParent
    {   
        public int ChildParentId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public char Relationship { get; set; } //F> Father, M>Mother

        #region Relations with another tables
        public Parent Parent { get; set; }
        public Child Child { get; set; }
        #endregion
    }
}
