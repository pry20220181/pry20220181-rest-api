using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class HealthPersonnel
    {
        public int HealthPersonnelId { get; set; }
        public string UserId { get; set; }

        #region Relations with another tables
        public User User { get; set; }
        #endregion
    }
}
