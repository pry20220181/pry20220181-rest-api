using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        #region Relations with another tables
        public Parent Parent { get; set; }
        public HealthPersonnel HealthPersonnel { get; set; }
        #endregion
    }
}
