using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Models
{
    public class HealthPersonnel
    {
        public int HealthPersonnelId { get; set; }
        public string DNI { get; set; }
        public string UserId { get; set; }
        public int HealthCenterId { get; set; }

        #region Relations with another tables
        public User User { get; set; }
        public HealthCenter HealthCenter { get; set; }
        #endregion
    }
}
